using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RpgBot.Command.Abstraction;
using RpgBot.Service.Abstraction;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace RpgBot.Bot.Telegram
{
    public class TelegramBot : BotRunner<Message, ChatId>
    {
        private ITelegramBotClient _bot;
        private readonly ILogger<TelegramBot> _logger;
        private readonly TelegramCommands _telegramCommands;
        private readonly IConfiguration _configuration;

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

        protected override Task<Message> SendMessageAsync(ChatId chat, string message)
        {
            return _bot.SendTextMessageAsync(chat, message);
        }

        private void OnMessage(object sender, MessageEventArgs args)
        {
            HandleMessage(
                args.Message.Text,
                args.Message.Chat,
                args.Message.From.Id.ToString(),
                args.Message.From.Username,
                args.Message.Chat.Id.ToString());
        }
    }
}