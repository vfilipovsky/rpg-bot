using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Service.Abstraction;
using SQLitePCL;

namespace RpgBot.Command
{
    public class MeCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const string Name = "/me";
        private const string Description = "Show details about you";

        private readonly IUserService _userService;

        public MeCommand(IUserService userService)
        {
            _userService = userService;
        }
        
        public string Run(string text, User user)
        {
            return _userService.Stringify(user);
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