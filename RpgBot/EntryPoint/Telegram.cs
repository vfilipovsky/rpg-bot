using RpgBot.Bot.Telegram;

namespace RpgBot.EntryPoint
{
    public class Telegram : IEntryPoint
    {
        private readonly TelegramBot _telegramBot;

        public Telegram(TelegramBot telegramBot)
        {
            _telegramBot = telegramBot;
        }

        public void Run(string[] args)
        {
            _telegramBot.Listen();
        }
    }
}