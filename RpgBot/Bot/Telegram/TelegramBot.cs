using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RpgBot.Command.Abstraction;
using RpgBot.Service.Abstraction;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using User = RpgBot.Entity.User;

namespace RpgBot.Bot.Telegram
{
    public class TelegramBot : BotRunner<Message, ChatId>
    {
        private ITelegramBotClient _bot;
        private readonly ILogger<TelegramBot> _logger;
        private readonly TelegramCommands _telegramCommands;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public TelegramBot(
            IUserService userService,
            ILogger<TelegramBot> logger,
            IConfiguration configuration,
            ICommands commands,
            TelegramCommands telegramCommands) : base(userService, logger, commands)
        {
            _logger = logger;
            _telegramCommands = telegramCommands;
            _configuration = configuration;
            _userService = userService;
        }

        public override void Listen()
        {
            _bot = new TelegramBotClient(_configuration["Bot:Token"]);
            _bot.SetMyCommandsAsync(_telegramCommands.List());
            _bot.OnMessage += OnMessage;
            _bot.StartReceiving();

            _logger.LogInformation("Bot started listening...");
            Console.In.ReadLineAsync();

            _bot.StopReceiving();
        }

        protected override Task<Message> SendMessageAsync(ChatId chat, string message, string messageId = null)
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

        protected override User Advance(User user, ChatId chat)
        {
            var userLevel = user.Level;
            _userService.AddExpForMessage(user);

            if (user.Level != userLevel)
                SendMessageAsync(chat, $"@{user.Username}, you have advanced to level {user.Level}!");

            return user;
        }

        private void OnMessage(object sender, MessageEventArgs args)
        {
            var text = ParseMessage(args.Message);

            HandleMessage(
                text,
                args.Message.Chat,
                args.Message.From.Id.ToString(),
                args.Message.From.Username ?? args.Message.From.Id.ToString(),
                args.Message.Chat.Id.ToString(),
                args.Message.MessageId.ToString());
        }

        private string ParseMessage(Message message)
        {
            if (message.Sticker != null) return "sticker";

            if (message.Photo != null && message.Text == null) return "image";
            
            var text = message.Text;

            if (text == null) return null;

            if (!text.StartsWith('/')) return text;

            var parts = text.Split(' ');

            if (parts.Length < 2) return text;

            if (parts[1].StartsWith('@')) return text;

            if (message.Entities == null) return text;
            
            foreach (var entity in message.Entities)
            {
                var u = entity.User;
                if (u == null) continue;

                var user = _userService.Get(u.Id.ToString(), u.Id.ToString());

                text = parts[0] + ' ' + user.Username;
            }

            return text;
        }
    }
}