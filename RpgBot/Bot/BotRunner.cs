﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Bot
{
    public abstract class BotRunner<T, TK>
    {
        private readonly IUserService _userService;
        private readonly ICommandAliasService _commandAliasService;
        private readonly ILogger _logger;
        private readonly ICommands _commands;

        protected BotRunner(
            IUserService userService,
            ILogger logger,
            ICommands commands,
            ICommandAliasService commandAliasService)
        {
            _userService = userService;
            _logger = logger;
            _commands = commands;
            _commandAliasService = commandAliasService;
        }

        public abstract void Listen();
        protected abstract Task<T> SendMessageAsync(TK chat, string message, string messageId = null);
        protected abstract User Advance(User user, TK chat);

        private ICommand GetCommand(string commandName)
        {
            var command = _commands.List().FirstOrDefault(c => c.Name == commandName);

            if (command != null)
                return command;

            var aliasCommand = _commandAliasService.Get(commandName.Replace("/", string.Empty));

            if (null == aliasCommand)
                throw new NotFoundException($"Command not found by name '{commandName}'");

            command = _commands.List().FirstOrDefault(c => c.Name == $"/{aliasCommand.Name}");

            if (null == command)
                throw new NotFoundException($"Command not found by name or alias: '{commandName}'");


            return command;
        }

        protected void HandleMessage(
            string message,
            TK chat,
            string userId,
            string username,
            string groupId,
            string messageId = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (username == null)
            {
                return;
            }

            try
            {
                var user = _userService.Get(username, userId);

                if (user.UserId != groupId) Advance(user, chat);

                if (!message.StartsWith('/'))
                {
                    return;
                }

                var commandName = message.Split(' ')[0];
                var command = GetCommand(commandName);

                if (command.LevelFrom > user.Level)
                {
                    SendMessageAsync(chat, $"Command available from level {command.LevelFrom}");
                    return;
                }

                SendMessageAsync(chat, command.Run(message, user), messageId);
            }
            catch (BotException e)
            {
                SendMessageAsync(chat, e.Message, messageId);
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.Message);
                SendMessageAsync(chat, "Unexpected error");
            }
        }
    }
}