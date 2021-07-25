using System.Collections.Generic;
using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class Commands : ICommands
    {
        private readonly PraiseCommand _praiseCommand;
        private readonly PunishCommand _punishCommand;
        private readonly TopCommand _topCommand;
        private readonly MeCommand _meCommand;
        private readonly AboutCommand _aboutCommand;
        private readonly CreateCommandAliasCommand _createCommandAliasCommand;
        private readonly DeleteCommandAliasCommand _deleteCommandAliasCommand;
        private readonly ListAliasesCommand _listAliasesCommand;

        private readonly ICommandAliasService _commandAliasService;

        public Commands(
            PraiseCommand praise,
            PunishCommand punish,
            TopCommand top,
            MeCommand me, 
            AboutCommand about,
            CreateCommandAliasCommand createCommandAlias,
            DeleteCommandAliasCommand deleteCommandAlias, 
            ListAliasesCommand listAliasesCommand,
            ICommandAliasService commandAliasService)
        {
            _praiseCommand = praise;
            _punishCommand = punish;
            _topCommand = top;
            _meCommand = me;
            _aboutCommand = about;
            _createCommandAliasCommand = createCommandAlias;
            _deleteCommandAliasCommand = deleteCommandAlias;
            _listAliasesCommand = listAliasesCommand;
            _commandAliasService = commandAliasService;
        }

        public static IEnumerable<string> ListNames()
        {
            return new[]
            {
                "praise",
                "me",
                "punish",
                "top",
                "about",
                "alias",
                "dalias",
                "aliases",
            };
        }
        
        public ICommand GetCommand(string commandName)
        {
            var command = List().FirstOrDefault(c => c.Name == commandName);

            if (command != null)
                return command;

            var aliasCommand = _commandAliasService.Get(commandName.Replace("/", string.Empty));

            if (null == aliasCommand)
                throw new NotFoundException($"Command not found by name '{commandName}'");

            command = List().FirstOrDefault(c => c.Name == $"/{aliasCommand.Name}");

            if (null == command)
                throw new NotFoundException($"Command not found by name or alias: '{commandName}'");


            return command;
        }
        
        public IEnumerable<ICommand> List()
        {
            return new List<ICommand>
            {
                _praiseCommand, 
                _meCommand, 
                _punishCommand, 
                _topCommand,
                _aboutCommand,
                _createCommandAliasCommand,
                _deleteCommandAliasCommand,
                _listAliasesCommand,
            };
        }

    }
}