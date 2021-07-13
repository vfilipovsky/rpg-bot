using System.Collections.Generic;
using RpgBot.Entity;

namespace RpgBot.Service.Abstraction
{
    public interface IUserService
    {
        public User Get(string username, string userId, string groupId);
        public User Create(string username, string userId, string groupId);
        public User AddExpForMessage(User user);
        public User Praise(string username, User user);
        public User Punish(string username, User user);
        public List<User> GetTopPlayers();
    }
}