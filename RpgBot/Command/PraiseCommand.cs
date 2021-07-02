using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public class PraiseCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 1;
        private const string Name = "/praise";
        private const string Description = "Praise user and give him +1 to reputation.";

        public string Run(string text)
        {
            var message = "123";
            var args = GetArgs(text, ArgsCount);
            
            return message;
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