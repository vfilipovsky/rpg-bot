using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class CreateCommandAliasCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/alias";
        public string Description { get; set; } = "Creates alias for a command ($commandName, $alias)";
        public int ArgsCount { get; set; } = 2;
        public int LevelFrom { get; set; } = 3;

        private readonly ICommandAliasService _commandAliasService;

        public CreateCommandAliasCommand(ICommandAliasService commandAliasService)
        {
            _commandAliasService = commandAliasService;
        }

        public string Run(string message, User user)
        {
            var args = GetArgs(message, ArgsCount);
            var commandName = args.ElementAt(1);
            var commandAlias = args.ElementAt(2);

            _commandAliasService.Create(commandAlias, commandName);

            return "Alias successfully created";
        }
    }
}