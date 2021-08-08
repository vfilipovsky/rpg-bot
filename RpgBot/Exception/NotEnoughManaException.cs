namespace RpgBot.Exception
{
    public class NotEnoughManaException : BotException
    {
        public NotEnoughManaException(string message) : base(message)
        {
        }
    }
}