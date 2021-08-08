namespace RpgBot.Exception
{
    public class PraiseYourselfException : BotException
    {
        public PraiseYourselfException(string message = "You cannot praise yourself") : base(message)
        {
        }
    }
}