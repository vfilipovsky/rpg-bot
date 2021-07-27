using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class AboutCommand : ICommand
    {
        public string Name => "/about";
        public string Description => "Show details about target user";
        public int ArgsCount => 1;
        public int RequiredLevel => 1;

        private readonly IUserService _userService;
        private readonly ICommandArgsResolver _commandArgsResolver;

        public AboutCommand(IUserService userService, ICommandArgsResolver commandArgsResolver)
        {
            _userService = userService;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string text, User user)
        {
            var username = _commandArgsResolver
                .GetArgs(text, ArgsCount)
                .ElementAt(1)
                .Replace("@", string.Empty);

            var userAbout = _userService.GetByUsername(username);

            return null == userAbout 
                ? $"User '{username}' not found." 
                : _userService.Stringify(userAbout);
        }
    }
}