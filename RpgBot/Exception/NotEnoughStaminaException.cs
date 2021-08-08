namespace RpgBot.Exception
{
    public class NotEnoughStaminaException : BotException
    {
        public NotEnoughStaminaException(string message) : base(message)
        {
        }
    }
}