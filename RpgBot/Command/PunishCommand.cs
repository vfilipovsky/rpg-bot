using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PunishCommand : ICommand
    {
        public string Name => "/punish";
        public string Description => "Punish @user and subtracts reputation from him";
        public int ArgsCount => 1;
        public int RequiredLevel => 3;

        private readonly IUserService _userService;
        private readonly IRate _rate;
        private readonly ICommandArgsResolver _commandArgsResolver;
        
        public PunishCommand(IUserService userService, IRate rate, ICommandArgsResolver commandArgsResolver)
        {
            _userService = userService;
            _rate = rate;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string text, User user)
        {
            var username = _commandArgsResolver.GetArgs(text, ArgsCount)
                .ElementAt(1)?
                .Replace('@'.ToString(), string.Empty);

            if (user.StaminaPoints < _rate.PunishStaminaCost)
                return $"Not enough stamina, need {_rate.PunishStaminaCost} ({user.StaminaPoints}).";
            
            var userToPunish = _userService.Punish(username, user);

            return null == userToPunish 
                ? $"User '{username}' not found" 
                : $"{userToPunish.Username} got punished. {userToPunish.Reputation} reputation in total.";
        }
    }
}