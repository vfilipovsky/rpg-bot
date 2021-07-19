using System.Collections.Generic;
using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBot.Service
{
    public class CommandAliasService : ICommandAliasService
    {
        private readonly BotContext _botContext;

        public CommandAliasService(BotContext botContext)
        {
            _botContext = botContext;
        }
        
        public CommandAlias Get(string alias)
        {
            return _botContext.CommandAliases.FirstOrDefault(c => c.Alias == alias);
        }

        public CommandAlias Create(string alias, string commandName)
        {
            var exists = Get(alias);

            if (exists != null)
            {
                throw new CommandAliasAlreadyExistsException($"Command with alias '{alias}' already exists");
            }

            var commandAlias = new CommandAlias() {Alias = alias, Name = commandName};

            _botContext.CommandAliases.Add(commandAlias);
            _botContext.SaveChanges();

            return commandAlias;
        }

        public CommandAlias Delete(CommandAlias commandAlias)
        {
            _botContext.CommandAliases.Remove(commandAlias);
            _botContext.SaveChanges();

            return commandAlias;
        }

        public IEnumerable<CommandAlias> List()
        {
            return _botContext.CommandAliases.ToList();
        }

        public CommandAlias Delete(string alias)
        {
            var exists = Get(alias);

            if (exists == null)
            {
                throw new NotFoundException($"Command with alias '{alias}' not found");
            }

            return Delete(exists);
        }
    }
}