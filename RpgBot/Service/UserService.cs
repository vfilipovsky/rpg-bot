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

        public UserService(BotContext botContext)
        {
            _context = botContext;
        }

        public User Create(string username, string userId, string groupId)
        {
            var user = new User()
            {
                GroupId = groupId,
                Username = username,
                UserId = userId,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetByUsernameAndGroupId(string username, string groupId)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username && u.GroupId == groupId);
        }

        public User GetByUserIdAndGroupId(string userId, string groupId)
        {
            return _context.Users
                .FirstOrDefault(u => u.UserId == userId && u.GroupId == groupId);
        }

        public User Get(string username, string userId, string groupId)
        {
            var user = GetByUsernameAndGroupId(username, groupId) ?? Create(username, userId, groupId);

            if (user.Username == username) return user;

            user.Username = username;

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        private User Regenerate(User user)
        {
            var hpAfterRegen = user.HealthPoints + Rate.HealthRegen;
            var manaAfterRegen = user.ManaPoints + Rate.ManaRegen;
            var staminaAfterRegen = user.StaminaPoints + Rate.StaminaRegen;

            if (hpAfterRegen <= user.MaxHealthPoints) user.HealthPoints = hpAfterRegen;
            if (manaAfterRegen <= user.MaxManaPoints) user.ManaPoints = manaAfterRegen;
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
            var userToPraise = GetByUsernameAndGroupId(username, user.GroupId);

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
            var userToPunish = GetByUsernameAndGroupId(username, user.GroupId);

            if (null == userToPunish) return null;

            user.StaminaPoints -= Rate.PunishStaminaCost;
            userToPunish.Reputation += Rate.ReputationPerPunish;

            _context.Users.Update(user);
            _context.Users.Update(userToPunish);
            _context.SaveChanges();

            return userToPunish;
        }

        public IEnumerable<User> GetTopPlayers(string groupId)
        {
            return _context.Users
                .Where(u => u.GroupId == groupId)
                .OrderByDescending(u => u.Level);
        }

        public string Stringify(User user)
        {
            return
                $"Id: {user.UserId}\n" +
                $"Username: {user.Username}\n" +
                $"Messages Count: {user.MessagesCount}\n" +
                $"Reputation: {user.Reputation}\n" +
                $"LVL: {user.Level}\n" +
                $"Exp: {user.Experience}/{LevelSystem.GetExpToNextLevel(user.Level)}\n" +
                $"HP: {user.HealthPoints}/{user.MaxHealthPoints}\n" +
                $"MP: {user.ManaPoints}/{user.MaxManaPoints}\n" +
                $"SP: {user.StaminaPoints}/{user.MaxStaminaPoints}";
        }
    }
}