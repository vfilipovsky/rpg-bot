using System.Collections;
using System.Collections.Generic;
using RpgBot.Entity;

namespace RpgBot.Service.Abstraction
{
    public interface ICommandAliasService
    {
        public CommandAlias Get(string alias);
        public CommandAlias Create(string alias, string commandName);
        public CommandAlias Delete(string alias);
        public CommandAlias Delete(CommandAlias commandAlias);
        public IEnumerable<CommandAlias> List();
    }
}