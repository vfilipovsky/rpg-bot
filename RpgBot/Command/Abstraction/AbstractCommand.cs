using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgBot.Command.Abstraction
{
    public abstract class AbstractCommand
    {
        protected IEnumerable<string> GetArgs(string text, int argsCount)
        {
            var parts = text.Split(' ');

            if (parts.Length < argsCount)
            {
                throw new ArgumentException("Wrong args, please see examples by starting type your command");
            }

            for (var i = 0; i < argsCount; i++)
            {
                var index = parts[i].IndexOf("\n", StringComparison.Ordinal);

                if (index > 0)
                {
                    parts[i] = parts[i][..index];
                }   
            }

            return parts.ToList();
        }
    }
}