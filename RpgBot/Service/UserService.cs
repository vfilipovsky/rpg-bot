using System.Collections.Generic;
using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Service.Abstraction;

namespace RpgBot.Service
{
    public class UserService : IUserService
    {
        private readonly BotContext _context;
        private readonly IGroupService _groupService;

        public UserService(BotContext botContext, IGroupService groupService)
        {
            _context = botContext;
            _groupService = groupService;
        }

        public User Create(string username, string userId, string groupId)
        {
            var group = _groupService.Get(groupId);

            var user = new User()
            {
                Group = group,
                Username = username,
                Id = userId,
            };

            _context.Groups.Attach(user.Group);
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Get(string username, string userId, string groupId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId && u.Group.Id == groupId)
                       ?? Create(username, userId, groupId);

            if (user.Username == username) return user;

            user.Username = username;
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        private User Regenerate(User user)
        {
            var hpAfterRegen = user.HealthPoints += Rate.HealthRegen;

            if (hpAfterRegen <= user.MaxHealthPoints) user.HealthPoints = hpAfterRegen;

            var manaAfterRegen = user.ManaPoints += Rate.ManaRegen;

            if (manaAfterRegen <= user.MaxManaPoints) user.ManaPoints = manaAfterRegen;

            var staminaAfterRegen = user.StaminaPoints += Rate.StaminaRegen;

            if (staminaAfterRegen <= user.MaxStaminaPoints) user.StaminaPoints = staminaAfterRegen;
            
            return user;
        }
        
        public User AddExpForMessage(User user)
        {
            LevelSystem.AddExp(user, Rate.ExpPerMessage);
            user.MessagesCount += 1;

            if (user.MessagesCount % Rate.RegeneratePerMessages == 0)
                user = Regenerate(user);

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User Praise(string username, User user)
        {
            var userToPraise = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Group.Id == user.Group.Id);
            
            if (null == userToPraise) return null;

            user.ManaPoints -= Rate.PraiseManaCost;
            userToPraise.Reputation += Rate.ReputationPerPraise;
            
            _context.Users.Update(user);
            _context.Users.Update(userToPraise);
            _context.SaveChanges();

            return userToPraise;
        }
        
        public User Punish(string username, User user)
        {
            var userToPunish = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Group.Id == user.Group.Id);
            
            if (null == userToPunish) return null;

            user.StaminaPoints -= Rate.PunishStaminaCost;
            userToPunish.Reputation += Rate.ReputationPerPunish;

            _context.Users.Update(user);
            _context.Users.Update(userToPunish);
            _context.SaveChanges();

            return userToPunish;
        }

        public IEnumerable<User> GetTopPlayers()
        {
            // todo: finish method
            return new List<User>();
            // return _context.Users
            //     .GroupBy(u => u.Level)
            //     .Select(u => u.OrderByDescending(y => y.Level)).ToList();
        }
    }
}