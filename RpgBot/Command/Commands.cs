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

        public Commands(
            PraiseCommand praise, 
            PunishCommand punish, 
            TopCommand top, 
            MeCommand me, 
            AboutCommand about,
            CreateCommandAliasCommand createCommandAlias,
            DeleteCommandAliasCommand deleteCommandAlias)
        {
            _praiseCommand = praise;
            _punishCommand = punish;
            _topCommand = top;
            _meCommand = me;
            _aboutCommand = about;
            _createCommandAliasCommand = createCommandAlias;
            _deleteCommandAliasCommand = deleteCommandAlias;
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
            };
        }
    }
}