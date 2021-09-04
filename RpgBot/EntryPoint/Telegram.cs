using RpgBot.Bot;
using Telegram.Bot.Types;

namespace RpgBot.EntryPoint
{
    public class Telegram : IEntryPoint
    {
        private readonly IBot<Message, ChatId> _telegramBot;

        public Telegram(IBot<Message, ChatId> telegramBot)
        {
            _telegramBot = telegramBot;
        }

        public void Run(string[] args)
        {
            _telegramBot.Listen();
        }
    }
}