using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PunishCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/punish";
        public string Description { get; set; } = "Punish @user and subtracts reputation from him";
        public int ArgsCount { get; set; } = 1;
        public int RequiredLevel { get; set; } = 3;

        private readonly IUserService _userService;
        private readonly IRate _rate;
        
        public PunishCommand(IUserService userService, IRate rate)
        {
            _userService = userService;
            _rate = rate;
        }
        
        public string Run(string text, User user)
        {
            var username = GetArgs(text, ArgsCount)
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