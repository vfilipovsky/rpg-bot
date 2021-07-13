using System.Threading.Tasks;
using RpgBot.Command;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Bot
{
    public abstract class BotRunner<T, TK>
    {
        private readonly IUserService _userService;

        protected BotRunner(IUserService userService)
        {
            _userService = userService;
        }
        
        public abstract void Listen();

        protected abstract Task<T> SendMessageAsync(TK chat, string message);

        protected void HandleMessage(string message, TK chat, string userId, string username, string group)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var user = _userService.Get(username, userId, group);
            
            try
            {
                if (!message.StartsWith('/'))
                {
                    _userService.AddExpForMessage(user);
                    return;
                }

                foreach (var command in Commands.List())
                {
                    if (!message.StartsWith(command.GetName())) continue;
                    SendMessageAsync(chat, command.Run(message));
                    return;
                }

                SendMessageAsync(chat, $"Command '{message}' not found.");
            }
            catch (BotException e)
            {
                SendMessageAsync(chat, e.Message);
            }
            catch (System.Exception e)
            {
                // todo: log
                // SendMessageAsync(chat, "Unexpected error");
                SendMessageAsync(chat, e.Message);
            }
        }
    }
}