using System.Collections.Generic;
using RpgBot.Entity;

namespace RpgBot.Service.Abstraction
{
    public interface IUserService
    {
        public User GetByUsernameAndGroupId(string username, string groupId);
        public User GetByUserIdAndGroupId(string userId, string groupId);
        public User Get(string username, string userId, string groupId);
        public User Create(string username, string userId, string groupId);
        public User AddExpForMessage(User user);
        public User Praise(string username, User user);
        public User Punish(string username, User user);
        public IEnumerable<User> GetTopPlayers(string groupId);
        public string Stringify(User user);
    }
}