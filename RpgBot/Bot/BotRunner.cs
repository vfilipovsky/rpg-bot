using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RpgBot.Command.Abstraction;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Bot
{
    public abstract class BotRunner<T, TK>
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly ICommands _commands;

        protected BotRunner(IUserService userService, ILogger logger, ICommands commands)
        {
            _userService = userService;
            _logger = logger;
            _commands = commands;
        }
        
        public abstract void Listen();

        protected abstract Task<T> SendMessageAsync(TK chat, string message);

        protected void HandleMessage(string message, TK chat, string userId, string username, string groupId)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (username == null && message.StartsWith('/'))
            {
                SendMessageAsync(chat, "Add username to your profile.");
                return;
            }

            if (username == null)
            {
                return;
            }
            
            var user = _userService.Get(username, userId, groupId);
            
            try
            {
                if (!message.StartsWith('/'))
                {
                    _userService.AddExpForMessage(user);
                    return;
                }

                foreach (var command in _commands.List())
                {
                    if (!message.StartsWith(command.GetName())) continue;
                    SendMessageAsync(chat, command.Run(message, user));
                    return;
                }

                SendMessageAsync(chat, $"Command '{message}' not found. @" + username);
            }
            catch (BotException e)
            {
                SendMessageAsync(chat, e.Message);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                SendMessageAsync(chat, "Unexpected error");
            }
        }
    }
}