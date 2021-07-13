using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level;

namespace RpgBot.Command
{
    public class MeCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const string Name = "/me";
        private const string Description = "Show details about you";
        
        public string Run(string text, User user)
        {
            return
                $"Username: @{user.Username}\n" +
                $"Reputation: {user.Reputation}\n" +
                $"Level: {user.Level}\n" +
                $"Experience: {user.Experience}/{LevelSystem.GetExpToNextLevel(user.Level)}\n";
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return Description;
        }

        public int GetArgsCount()
        {
            return ArgsCount;
        }
    }
}