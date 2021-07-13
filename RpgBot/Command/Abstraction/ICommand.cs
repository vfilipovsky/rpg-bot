using RpgBot.Entity;

namespace RpgBot.Command.Abstraction
{
    public interface ICommand
    {
        public string Run(string message, User user);
        public string GetName();
        public string GetDescription();
        public int GetArgsCount();
    }
}