using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class MeCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/me";
        public string Description { get; set; } = "Show details about you";
        public int ArgsCount { get; set; } = 0;
        public int LevelFrom { get; set; } = 1;

        private readonly IUserService _userService;

        public MeCommand(IUserService userService)
        {
            _userService = userService;
        }
        
        public string Run(string text, User user)
        {
            return _userService.Stringify(user);
        }
    }
}