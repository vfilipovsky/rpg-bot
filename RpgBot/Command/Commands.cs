using System.Collections.Generic;
using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public static class Commands
    {
        public static IEnumerable<ICommand> List()
        {
            return new List<ICommand>
            {
                new PraiseCommand(), 
                new MeCommand(), 
                new PunishCommand(), 
                new TopCommand(),
                new StartCommand(),
            };
        }
    }
}