using System;
using TelegramRpgBot.Bot;

namespace TelegramRpgBot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();

            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

            if (null == token)
            {
                throw new ArgumentException("'token' not provided in .env");
            }
            
            new TelegramBot(token).Listen();
        }
    }
}