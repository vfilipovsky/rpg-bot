using System.Collections.Generic;
using RpgBot.Entity;
using RpgBot.Type;

namespace RpgBot.Service.Abstraction
{
    public interface IUserService
    {
        public User GetByUsername(string username);
        public User GetByUserId(string userId);
        public User Get(string username, string userId);
        public User Create(string username, string userId);
        public User AddExpForMessage(User user, MessageType type);
        public User Praise(string username, User user);
        public User Punish(string username, User user);
        public IEnumerable<User> GetTopPlayers();
        public string Stringify(User user);
    }
}