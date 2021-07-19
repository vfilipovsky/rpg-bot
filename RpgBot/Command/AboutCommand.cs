using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class AboutCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/about";
        public string Description { get; set; } = "Show details about target user";
        public int ArgsCount { get; set; } = 1;
        public int LevelFrom { get; set; } = 1;

        private readonly IUserService _userService;

        public AboutCommand(IUserService userService)
        {
            _userService = userService;
        }
        
        public string Run(string text, User user)
        {
            var username = GetArgs(text, ArgsCount).ElementAt(1).Replace("@", string.Empty);
            var userAbout = _userService.GetByUsername(username);

            return null == userAbout 
                ? $"User '{username}' not found." 
                : _userService.Stringify(userAbout);
        }
    }
}