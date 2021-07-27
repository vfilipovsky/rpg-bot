using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class MeCommand : ICommand
    {
        public string Name => "/me";
        public string Description => "Show details about you";
        public int ArgsCount => 0;
        public int RequiredLevel => 1;

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