using System;
using System.Collections.Generic;
using System.Linq;
using RpgBot.Exception;

namespace RpgBot.Command.Abstraction
{
    public abstract class AbstractCommand
    {
        public virtual IEnumerable<string> GetArgs(string text, int argsCount)
        {
            var parts = text.Split(' ');

            if (parts.Length < argsCount + 1)
            {
                throw new BotException("Pass the argument to command");
            }

            return ClearArgs(parts, argsCount).ToList();
        }

        public string[] ClearArgs(string[] parts, int argsCount)
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