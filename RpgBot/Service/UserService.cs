using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Service.Abstraction;

namespace RpgBot.Service
{
    public class UserService : IUserService
    {
        private readonly BotContext _botContext;
        private readonly IGroupService _groupService;

        public UserService(BotContext botContext, IGroupService groupService)
        {
            _botContext = botContext;
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

            _botContext.Users.Add(user);
            _botContext.SaveChanges();

            return user;
        }

        public User Get(string username, string userId, string groupId)
        {
            return _botContext.Users.FirstOrDefault(u => u.Id == userId && u.Group.Id == groupId)
                   ?? Create(username, userId, groupId);
        }

        public User AddExpForMessage(User user)
        {
            LevelSystem.AddExp(user, Rate.ExpPerMessage);
            _botContext.Users.Update(user);
            _botContext.SaveChanges();

            return user;
        }
    }
}