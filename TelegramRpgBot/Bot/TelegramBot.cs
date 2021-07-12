using System;
using System.Threading.Tasks;
using RpgBot.Bot;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramRpgBot.Bot
{
    public class TelegramBot : BotRunner<Message, ChatId>
    {
        private readonly ITelegramBotClient _bot;

        public TelegramBot(string token)
        {
            _bot = new TelegramBotClient(token); 
        }
        
        public override void Listen()
        {
            _bot.SetMyCommandsAsync(TelegramCommands.List());
            _bot.OnMessage += OnMessage;
            _bot.StartReceiving();

            Console.WriteLine("Bot is running...");
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
