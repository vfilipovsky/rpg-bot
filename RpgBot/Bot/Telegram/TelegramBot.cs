using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RpgBot.Command.Abstraction;
using RpgBot.DTO;
using RpgBot.EntryPoint;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;
using RpgBot.Type;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using User = RpgBot.Entity.User;

namespace RpgBot.Bot.Telegram
{
    public class TelegramBot : IBot<Message, ChatId>, IEntryPoint
    {
        private ITelegramBotClient _bot;
        private readonly ILogger<TelegramBot> _logger;
        private readonly TelegramCommands _telegramCommands;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IExperienceService _experienceService;
        private readonly ICommands _commands;

        public TelegramBot(
            IUserService userService,
            ILogger<TelegramBot> logger,
            IConfiguration configuration,
            ICommands commands,
            IExperienceService experienceService,
            TelegramCommands telegramCommands)
        {
            _logger = logger;
            _telegramCommands = telegramCommands;
            _configuration = configuration;
            _userService = userService;
            _experienceService = experienceService;
            _commands = commands;
        }

        public void Run(string[] args)
        {
            Listen();
        }

        public void Listen()
        {
            _bot = new TelegramBotClient(_configuration["Bot:Token"]);
            _bot.SetMyCommandsAsync(_telegramCommands.List());
            _bot.OnMessage += HandleMessage;
            _bot.StartReceiving();

            _logger.LogInformation("Bot started listening...");

            while (String.Empty != Console.ReadLine()) { }

            _bot.StopReceiving();
        }

        public Task<Message> SendMessageAsync(ChatId chat, string message, string messageId = null)
        {
            if (null == messageId)
                return _bot.SendTextMessageAsync(chat, message, disableNotification: true);

            return _bot.SendTextMessageAsync(
                chat,
                message,
                replyToMessageId: int.Parse(messageId),
                disableNotification: true
            );
        }

        public User Advance(User user, ChatId chat, MessageType type)
        {
            var userLevel = user.Level;
            _experienceService.AddExpForMessage(user, type);

            if (user.Level != userLevel)
                SendMessageAsync(chat, $"@{user.Username}, you have advanced to level {user.Level}!");

            return user;
        }

        private void HandleMessage(object sender, MessageEventArgs args)
        {
            var dto = ParseMessage(args.Message);

            if (dto == null) return;

            try
            {
                var user = _userService.Get(dto.Username, dto.UserId);

                if (dto.GroupId == _configuration["Bot:GroupId"]) Advance(user, dto.Chat, dto.MessageType);

                if (!dto.Text.StartsWith('/'))
                {
                    return;
                }

                var commandName = dto.Text.Split(' ')[0];
                var command = _commands.GetCommand(commandName);

                if (command.RequiredLevel > user.Level)
                {
                    SendMessageAsync(dto.Chat, $"Command available from level {command.RequiredLevel}");
                    return;
                }

                SendMessageAsync(dto.Chat, command.Run(dto.Text, user), dto.MessageId);
            }
            catch (NotFoundException) { }
            catch (BotException e)
            {
                SendMessageAsync(dto.Chat, e.Message, dto.MessageId);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                SendMessageAsync(dto.Chat, "Unexpected error");
            }
        }

        public MessageDto<ChatId> ParseMessage(Message message)
        {
            var dto = new MessageDto<ChatId>()
            {
                Chat = message.Chat.Id,
                MessageType = MessageType.Text,
                MessageId = message.MessageId.ToString(),
                GroupId = message.Chat.Id.ToString(),
                Text = message.Text ?? string.Empty,
                Username = message.From.Username,
                UserId = message.From.Id.ToString(),
            };

            if (message.Sticker != null)
            {
                dto.MessageType = MessageType.Sticker;
                return dto;
            }

            if (message.Photo != null && message.Text == null)
            {
                dto.MessageType = MessageType.Image;
                return dto;
            }

            var text = message.Text;

            if (text == null) return null;

            if (!text.StartsWith('/')) return dto;

            var parts = text.Split(' ');

            if (parts[0].Contains('@')) parts[0] = parts[0].Split('@')[0];

            if (parts.Length < 2)
            {
                dto.Text = parts[0];
                return dto;
            }

            if (parts[1].StartsWith('@'))
            {
                dto.Text = string.Join(' ', parts);
                return dto;
            }

            if (message.Entities == null)
            {
                dto.Text = string.Join(' ', parts);
                return dto;
            }

            foreach (var entity in message.Entities)
            {
                var u = entity.User;
                if (u == null) continue;

                var user = _userService.Get(u.Id.ToString(), u.Id.ToString());

                parts[0] += ' ' + user.Username;
            }

            dto.Text = string.Join(' ', parts);
            return dto;
        }
    }
}