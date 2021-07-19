using System.Linq;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class DeleteCommandAliasCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/delete-alias";
        public string Description { get; set; } = "Removes alias";
        public int ArgsCount { get; set; } = 1;
        public int LevelFrom { get; set; } = 3;

        private readonly ICommandAliasService _commandAliasService;

        public DeleteCommandAliasCommand(ICommandAliasService commandAliasService)
        {
            _commandAliasService = commandAliasService;
        }
        
        public string Run(string message, User user)
        {
            var commandAlias = GetArgs(message, ArgsCount).ElementAt(1);
            _commandAliasService.Delete(commandAlias);
            return "Alias successfully removed";
        }
    }
}