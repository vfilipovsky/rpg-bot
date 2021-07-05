using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public class StartCommand : AbstractCommand, ICommand
    {
        private const string Name = "/start";
        private const string Description = "Starts and setups bot for your group";
        private const int ArgsCount = 0;
        
        public string Run(string message)
        {
            // TODO: finish command
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