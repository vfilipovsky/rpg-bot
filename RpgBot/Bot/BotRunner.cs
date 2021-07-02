using System.Threading.Tasks;
using RpgBot.Command;

namespace RpgBot.Bot
{
    public abstract class BotRunner<T, TK>
    {
        public abstract void Listen();

        protected abstract Task<T> SendMessageAsync(TK chat, string message);

        protected void HandleMessage(string message, TK chat, string username, string group)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (!message.StartsWith('/'))
            {
                // TODO: handle message to save xp
                return;
            }

            try
            {
                foreach (var command in Commands.List())
                {
                    if (!message.StartsWith(command.GetName())) continue;
                    SendMessageAsync(chat, command.Run(message));
                    return;
                }

                SendMessageAsync(chat, $"Command '{message}' not found.");
            }
            catch (System.Exception e)
            {
                SendMessageAsync(chat, e.Message);
            }
        }
    }
}