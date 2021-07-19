using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class ListAliasesCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/aliases";
        public string Description { get; set; } = "Lists all aliases";
        public int ArgsCount { get; set; } = 0;
        public int RequiredLevel { get; set; } = 1;

        private readonly ICommandAliasService _commandAliasService;

        public ListAliasesCommand(ICommandAliasService commandAliasService)
        {
            _commandAliasService = commandAliasService;
        }
        
        public string Run(string message, User user)
        {
            var aliases = _commandAliasService.List();
            return string.Join("\n", aliases.Select(a => $"{a.Alias} -> {a.Name}"));
        }
    }
}