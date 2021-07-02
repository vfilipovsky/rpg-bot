namespace RpgBot.Command.Abstraction
{
    public interface ICommand
    {
        public string Run(string message);
        public string GetName();
        public string GetDescription();
        public int GetArgsCount();
    }
}