namespace RpgBot.Exception
{
    public class CommandAliasAlreadyExistsException : BotException
    {
        public CommandAliasAlreadyExistsException(string message) : base(message)
        {
        }
    }
}