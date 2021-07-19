namespace RpgBot.Exception
{
    public class NotFoundException : BotException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}