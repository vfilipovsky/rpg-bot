using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class AboutCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 1;
        private const string Name = "/about";
        private const string Description = "Show details about target user";

        private readonly IUserService _userService;

        public AboutCommand(IUserService userService)
        {
            _userService = userService;
        }
        
        public string Run(string text, User user)
        {
            var username = GetArgs(text, ArgsCount)                
                .ElementAt(1)
                .Replace('@'.ToString(), string.Empty);

            var userAbout = _userService.GetByUsernameAndGroupId(username, user.Group.Id);

            if (null == userAbout) return $"User '{username}' not found. @{user.Username}";
            
            return _userService.Stringify(userAbout) + $"\n@{user.Username}";

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