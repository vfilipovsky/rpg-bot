using System.Collections.Generic;
using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public static class Commands
    {
        public static IEnumerable<ICommand> List()
        {
            var commands = new List<ICommand>
            {
                new PraiseCommand(), 
                new MeCommand(), 
                new PunishCommand(), 
                new TopCommand()
            };


            return commands;
        }
    }
}