using System.Collections.Generic;
using System.Linq;
using RpgBot.Command.Abstraction;
using Telegram.Bot.Types;

namespace RpgBot.Bot.Telegram
{
    public class TelegramCommands
    {
        private readonly ICommands _commands;

        public TelegramCommands(ICommands commands)
        {
            _commands = commands;
        }
        
        public IEnumerable<BotCommand> List()
        {
            return _commands
                .List()
                .Select(
                    command => new BotCommand {Command = command.Name, Description = command.Description})
                .ToList();
        }
    }
}