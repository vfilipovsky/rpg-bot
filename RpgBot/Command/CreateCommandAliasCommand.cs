using System.Collections.Generic;
using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class CreateCommandAliasCommand : ICommand
    {
        public string Name => "/alias";
        public string Description => "Creates alias for a command ($alias, $name)";
        public int ArgsCount => 2;
        public int RequiredLevel => 1;

        private readonly ICommandAliasService _commandAliasService;
        private readonly ICommandArgsResolver _commandArgsResolver;

        public CreateCommandAliasCommand(
            ICommandAliasService commandAliasService,
            ICommandArgsResolver commandArgsResolver)
        {
            _commandAliasService = commandAliasService;
            _commandArgsResolver = commandArgsResolver;
        }

        public string Run(string message, User user)
        {
            var args = _commandArgsResolver.GetArgs(message, ArgsCount);
            var enumerable = args.ToList();
            var commandName = enumerable.ElementAt(2);
            var commandAlias = enumerable.ElementAt(1);

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