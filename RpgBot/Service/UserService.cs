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

        public User AddExpForMessage(User user)
        {
            LevelSystem.AddExp(user, Rate.ExpPerMessage);
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public User Praise(string username, User user)
        {
            var userToPraise = _context.Users.FirstOrDefault(u => u.Username == username && u.Group == user.Group);
            
            if (null == userToPraise) return null;

            userToPraise.Experience += Rate.ReputationPerPraise;
            _context.Users.Update(userToPraise);
            _context.SaveChanges();

            return userToPraise;
        }
        
        public User Punish(string username, User user)
        {
            var userToPunish = _context.Users.FirstOrDefault(u => u.Username == username && u.Group == user.Group);
            
            if (null == userToPunish) return null;

            userToPunish.Experience += Rate.ReputationPerPunish;
            _context.Users.Update(userToPunish);
            _context.SaveChanges();

            return userToPunish;
        }

        public List<User> GetTopPlayers()
        {
            return _context.Users
                .GroupBy(u => u.Level)
                .Select(u => u.OrderByDescending(y => y.Level).First()).ToList();
        }
    }
}