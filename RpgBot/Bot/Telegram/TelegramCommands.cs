using System.Collections.Generic;
using System.Linq;
using RpgBot.Command;
using Telegram.Bot.Types;

namespace RpgBot.Bot.Telegram
{
    public static class TelegramCommands
    {
        public static IEnumerable<BotCommand> List()
        {
            return Commands
                .List()
                .Select(
                    command => new BotCommand {Command = command.GetName(), Description = command.GetDescription()})
                .ToList();
        }
    }
}