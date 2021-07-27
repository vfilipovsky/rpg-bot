using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class DeleteCommandAliasCommand : ICommand
    {
        public string Name => "/dalias";
        public string Description => "Remove alias";
        public int ArgsCount => 1;
        public int RequiredLevel => 1;

        private readonly ICommandAliasService _commandAliasService;
        private readonly ICommandArgsResolver _commandArgsResolver;

        public DeleteCommandAliasCommand(
            ICommandAliasService commandAliasService,
            ICommandArgsResolver commandArgsResolver)
        {
            _commandAliasService = commandAliasService;
            _commandArgsResolver = commandArgsResolver;
        }
        
        public string Run(string message, User user)
        {
            var commandAlias = _commandArgsResolver.GetArgs(message, ArgsCount).ElementAt(1);
            _commandAliasService.Delete(commandAlias);
            return "Alias successfully removed";
        }
    }
}