using System;
using RpgBot.Bot.Telegram;
using RpgBot.Service.Abstraction;

namespace RpgBot.EntryPoint
{
    public class Telegram : IEntryPoint
    {
        private readonly IUserService _userService;
        
        public Telegram(IUserService userService)
        {
            _userService = userService;
        }
        
        public void Run(string[] args)
        {
            DotNetEnv.Env.TraversePath().Load();

            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");

            if (null == token)
            {
                throw new ArgumentException("'token' not provided in .env");
            }

            new TelegramBot(token, _userService).Listen();
        }
    }
}