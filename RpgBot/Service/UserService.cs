using System.Collections.Generic;
using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Service
{
    public class UserService : IUserService
    {
        private readonly BotContext _context;
        private readonly IRate _rate;
        private readonly ILevelSystem _levelSystem;

        public UserService(BotContext botContext, IRate rate, ILevelSystem levelSystem)
        {
            _context = botContext;
            _rate = rate;
            _levelSystem = levelSystem;
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
            var user = GetByUserIdAndGroupId(userId, groupId) ?? Create(username, userId, groupId);

            if (user.Username == username) return user;

            user.Username = username ?? (user.Username = user.Id.ToString());

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        private User Regenerate(User user)
        {
            var hpAfterRegen = user.HealthPoints + _rate.HealthRegen;
            var manaAfterRegen = user.ManaPoints + _rate.ManaRegen;
            var staminaAfterRegen = user.StaminaPoints + _rate.StaminaRegen;

            if (hpAfterRegen <= user.MaxHealthPoints) user.HealthPoints = hpAfterRegen;
            if (manaAfterRegen <= user.MaxManaPoints) user.ManaPoints = manaAfterRegen;
            if (staminaAfterRegen <= user.MaxStaminaPoints) user.StaminaPoints = staminaAfterRegen;

            return user;
        }

        public User AddExpForMessage(User user)
        {
            _levelSystem.AddExp(user, _rate.ExpPerMessage);
            user.MessagesCount += 1;

            if (user.MessagesCount % _rate.RegeneratePerMessages == 0) 
                user = Regenerate(user);

            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User Praise(string username, User user)
        {
            var userToPraise = GetByUsernameAndGroupId(username, user.GroupId);

            if (null == userToPraise) return null;

            user.ManaPoints -= _rate.PraiseManaCost;
            userToPraise.Reputation += _rate.ReputationPerPraise;

            _context.Users.Update(user);
            _context.Users.Update(userToPraise);
            _context.SaveChanges();

            return userToPraise;
        }

        public User Punish(string username, User user)
        {
            var userToPunish = GetByUsernameAndGroupId(username, user.GroupId);

            if (null == userToPunish) return null;

            user.StaminaPoints -= _rate.PunishStaminaCost;
            userToPunish.Reputation += _rate.ReputationPerPunish;

            _context.Users.Update(user);
            _context.Users.Update(userToPunish);
            _context.SaveChanges();

            return userToPunish;
        }

        public IEnumerable<User> GetTopPlayers(string groupId)
        {
            return _context.Users
                .Where(u => u.GroupId == groupId)
                .OrderByDescending(u => u.Level)
                .ThenByDescending(u => u.Reputation)
                .ThenByDescending(u => u.Experience);
        }

        public string Stringify(User user)
        {
            return
                $"Id: {user.UserId}\n" +
                $"Name: {user.Username}\n" +
                $"Msg: {user.MessagesCount}\n" +
                $"Rep: {user.Reputation}\n" +
                $"LVL: {user.Level}\n" +
                $"Exp: {user.Experience}/{_levelSystem.GetExpToNextLevel(user.Level)}\n" +
                $"HP: {user.HealthPoints}/{user.MaxHealthPoints}\n" +
                $"MP: {user.ManaPoints}/{user.MaxManaPoints}\n" +
                $"SP: {user.StaminaPoints}/{user.MaxStaminaPoints}";
        }
    }
}