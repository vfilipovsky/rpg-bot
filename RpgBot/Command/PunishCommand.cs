using RpgBot.Command.Abstraction;

namespace RpgBot.Command
{
    public class PunishCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 1;
        private const string Name = "/punish";
        private const string Description = "Punish @user and substracts 1 reputation from him";
        
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