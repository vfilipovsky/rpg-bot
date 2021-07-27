using System;
using System.Collections.Generic;
using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Exception;

namespace RpgBot.Command
{
    public class CommandArgsResolver : ICommandArgsResolver
    {
        public IEnumerable<string> GetArgs(string text, int argsCount)
        {
            var parts = text.Split(' ');

            if (parts.Length < argsCount + 1)
            {
                throw new BotException("Pass the argument to command");
            }

            return ClearArgs(parts, argsCount).ToList();
        }

        public IEnumerable<string> ClearArgs(string[] parts, int argsCount)
        {
            for (var i = 0; i < argsCount; i++)
            {
                var index = parts[i].IndexOf("\n", StringComparison.Ordinal);

                if (index > 0)
                {
                    parts[i] = parts[i][..index];
                }
            }

            return parts;
        }
    }
}