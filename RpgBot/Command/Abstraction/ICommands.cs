using System.Collections.Generic;

namespace RpgBot.Command.Abstraction
{
    public interface ICommands
    {
        public IEnumerable<ICommand> List();
        public ICommand GetCommand(string commandName);
    }
}