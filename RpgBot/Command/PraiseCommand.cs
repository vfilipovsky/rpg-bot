using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PraiseCommand : ICommand
    {
        public string Name => "/praise";
        public string Description => "Praise user and give him +1 to reputation.";
        public int ArgsCount => 1;
        public int RequiredLevel => 2;

        private readonly IUserService _userService;
        private readonly IRate _rate;
        private readonly ICommandArgsResolver _commandArgsResolver;
        
        public PraiseCommand(IUserService userService, IRate rate, ICommandArgsResolver commandArgsResolver)
        {
            _userService = userService;
            _rate = rate;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string text, User user)
        {
            var username = _commandArgsResolver.GetArgs(text, ArgsCount)
                .ElementAt(1)
                .Replace('@'.ToString(), string.Empty);

            if (username == user.Username) return $"You cannot praise yourself";

            if (user.ManaPoints < _rate.PraiseManaCost)
                return $"Not enough mana, need {_rate.PraiseManaCost} ({user.ManaPoints}).";
            
            var userToPraise = _userService.Praise(username, user);

            return null == userToPraise 
                ? $"User '{username}' not found" 
                : $"{userToPraise.Username} got praised. {userToPraise.Reputation} reputation in total.";
        }
    }
}