using RpgBot.Entity;

namespace RpgBot.Command.Abstraction
{
    public interface ICommand
    {
        public string Run(string message, User user);
        public string Name { get; set; }
        public string Description { get; set; }
        public int ArgsCount { get; set; }
        public int LevelFrom { get; set; }
    }
}