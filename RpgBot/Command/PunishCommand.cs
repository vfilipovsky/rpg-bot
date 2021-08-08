using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PunishCommand : ICommand
    {
        public string Name => "/punish";
        public string Description => "Punish @user and subtracts reputation from him";
        public int ArgsCount => 1;
        public int RequiredLevel => 3;

        private readonly IExperienceService _experienceService;
        private readonly ICommandArgsResolver _commandArgsResolver;
        
        public PunishCommand(IExperienceService experienceService, ICommandArgsResolver commandArgsResolver)
        {
            _experienceService = experienceService;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string text, User user)
        {
            var username = _commandArgsResolver.GetArgs(text, ArgsCount)
                .ElementAt(1)?
                .Replace('@'.ToString(), string.Empty);

            var userToPunish = _experienceService.Punish(username, user);

            return $"{userToPunish.Username} got punished. {userToPunish.Reputation} reputation in total.";
        }
    }
}