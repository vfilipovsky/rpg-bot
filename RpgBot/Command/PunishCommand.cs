using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PunishCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 1;
        private const int LevelFrom = 3;
        private const string Name = "/punish";
        private const string Description = "Punish @user and substracts 1 reputation from him";
        
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

        public int GetLevelFrom()
        {
            return LevelFrom;
        }
    }
}