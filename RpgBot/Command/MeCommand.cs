using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public class MeCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const string Name = "/me";
        private const string Description = "Show details about you";
        
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