using System.Collections.Generic;

namespace RpgBot.Command.Abstraction
{
    public interface ICommandArgsResolver
    {
        public IEnumerable<string> GetArgs(string text, int argsCount);
        public IEnumerable<string> ClearArgs(string[] parts, int argsCount);
    }
}