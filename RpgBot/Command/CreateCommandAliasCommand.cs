using System.Collections.Generic;
using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class CreateCommandAliasCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/alias";
        public string Description { get; set; } = "Creates alias for a command ($alias, $name)";
        public int ArgsCount { get; set; } = 2;
        public int RequiredLevel { get; set; } = 1;

        private readonly ICommandAliasService _commandAliasService;

        public CreateCommandAliasCommand(ICommandAliasService commandAliasService)
        {
            _commandAliasService = commandAliasService;
        }

        public string Run(string message, User user)
        {
            var args = GetArgs(message, ArgsCount);
            var commandName = args.ElementAt(2);
            var commandAlias = args.ElementAt(1);

            ValidateAlias(commandAlias);

            var exists = Commands.ListNames().FirstOrDefault(c => c == commandAlias);

            if (exists != null)
                throw new AliasValidationException("You cant name alias by existing command");
            
            var existsCommand = Commands.ListNames().FirstOrDefault(c => c == commandName);

            if (existsCommand == null)
                throw new NotFoundException($"Command not found by name '{commandName}'");

            _commandAliasService.Create(commandAlias, commandName);

            return "Alias successfully created";
        }

        private static void ValidateAlias(string alias)
        {
            var rules = new List<string>()
            {
                "@"
            };

            if (rules.Contains(alias))
                throw new AliasValidationException(
                    $"Alias must not contain '{string.Join(", ", rules)}'"
                );
        }
    }
}