using RpgBot.Entity;

namespace RpgBot.Command.Abstraction
{
    public interface ICommand
    {
        public string Run(string message, User user);
        public string Name { get; }
        public string Description { get; }
        public int ArgsCount { get; }
        public int RequiredLevel { get; }
        
    }
}