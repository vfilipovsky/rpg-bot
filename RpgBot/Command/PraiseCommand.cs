using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PraiseCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 1;
        private const string Name = "/praise";
        private const string Description = "Praise user and give him +1 to reputation.";
        
        private readonly IUserService _userService;
        
        public PraiseCommand( IUserService userService)
        {
            _userService = userService;
        }
        
        public string Run(string text, User user)
        {
            var username = GetArgs(text, ArgsCount)
                .ElementAt(1)
                .Replace('@'.ToString(), string.Empty);

            if (username == user.Username) return $"You cannot praise yourself";

            if (user.ManaPoints < Rate.PraiseManaCost)
                return $"Not enough mana, need {Rate.PraiseManaCost} ({user.ManaPoints}).";
            
            var userToPraise = _userService.Praise(username, user);

            return null == userToPraise 
                ? $"User '{username}' not found" 
                : $"{userToPraise.Username} got praised. {userToPraise.Reputation} reputation in total.";
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return Description;
        }

        public int GetArgsCount()
        {
            return ArgsCount;
        }
    }
}