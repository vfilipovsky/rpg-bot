using System.Collections;
using System.Collections.Generic;
using RpgBot.Command.Abstraction;

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

        public Commands(
            PraiseCommand praise,
            PunishCommand punish,
            TopCommand top,
            MeCommand me, 
            AboutCommand about,
            CreateCommandAliasCommand createCommandAlias,
            DeleteCommandAliasCommand deleteCommandAlias, 
            ListAliasesCommand listAliasesCommand)
        {
            _praiseCommand = praise;
            _punishCommand = punish;
            _topCommand = top;
            _meCommand = me;
            _aboutCommand = about;
            _createCommandAliasCommand = createCommandAlias;
            _deleteCommandAliasCommand = deleteCommandAlias;
            _listAliasesCommand = listAliasesCommand;
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
                "lalias",
            };
        }
        
    }
}