using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public class TopCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const string Name = "/top";
        private const string Description = "List of players";
        
        public string Run(string text)
        {
            throw new System.NotImplementedException();
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