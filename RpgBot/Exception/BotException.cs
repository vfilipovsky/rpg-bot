namespace RpgBot.Exception
{
    public class BotException : System.Exception
    {
        protected BotException(string message)
            : base(message)
        {
        }
    }
}