namespace RpgBot.Exception
{
    public class BotException : System.Exception
    {
        public BotException(string message)
            : base(message)
        {
        }

        public BotException()
        {
        }
    }
}