using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class PraiseCommand : ICommand
    {
        public string Name => "/praise";
        public string Description => "Praise user and give him +1 to reputation.";
        public int ArgsCount => 1;
        public int RequiredLevel => 2;

        private readonly IExperienceService _experienceService;
        private readonly ICommandArgsResolver _commandArgsResolver;
        
        public PraiseCommand(IExperienceService experienceService, ICommandArgsResolver commandArgsResolver)
        {
            _experienceService = experienceService;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string text, User user)
        {
            var username = _commandArgsResolver.GetArgs(text, ArgsCount)
                .ElementAt(1)
                .Replace('@'.ToString(), string.Empty);

            if (username == user.Username) 
                throw new PraiseYourselfException();

            var userToPraise = _experienceService.Praise(username, user);

            return $"{userToPraise.Username} got praised. {userToPraise.Reputation} reputation in total.";
        }
    }
}