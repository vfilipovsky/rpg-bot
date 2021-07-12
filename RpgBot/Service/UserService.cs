using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Level;

namespace RpgBot.Service
{
    public class UserService
    {
        private readonly BotContext _context;
        private readonly GroupService _groupService;

        public UserService()
        {
            // TODO: DI
            _context = new BotContext();
            _groupService = new GroupService();
        }

        private User Create(string username, string userId, string groupId)
        {
            var group = _groupService.Get(groupId);

            var user = new User()
            {
                Group = group,
                Username = username,
                Id = userId,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public User Get(string username, string userId, string groupId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId && u.Group.Id == groupId)
                   ?? Create(username, userId, groupId);
        }

        public User AddExpForMessage(User user)
        {
            LevelSystem.AddExp(user, Rate.ExpPerMessage);
            _context.Users.Update(user);
            _context.SaveChangesAsync();

            return user;
        }
    }
}