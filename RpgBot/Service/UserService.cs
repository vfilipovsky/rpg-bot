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
        private readonly ILevelSystem _levelSystem;

        public UserService(BotContext botContext, ILevelSystem levelSystem)
        {
            _context = botContext;
            _levelSystem = levelSystem;
        }

        public User Create(string username, string userId)
        {
            var user = new User()
            {
                Username = username,
                UserId = userId,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User GetByUsername(string username)
        {
            return _context.Users
                .FirstOrDefault(u => u.Username == username);
        }

        public User GetByUserId(string userId)
        {
            return _context.Users
                .FirstOrDefault(u => u.UserId == userId);
        }

        public User Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }
        
        public User Get(string username, string userId)
        {
            var user = GetByUserId(userId) ?? Create(username, userId);

            if (user.Username == username) return user;

            user.Username = username ?? (user.Username = user.UserId);

            return Update(user);
        }

        public IEnumerable<User> GetTopPlayers()
        {
            return _context.Users
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