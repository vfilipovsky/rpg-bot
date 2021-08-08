using System.Collections.Generic;
using RpgBot.Entity;

namespace RpgBot.Service.Abstraction
{
    public interface IUserService
    {
        public User Update(User user);
        public User GetByUsername(string username);
        public User GetByUserId(string userId);
        public User Get(string username, string userId);
        public User Create(string username, string userId);
        public IEnumerable<User> GetTopPlayers();
        public string Stringify(User user);
    }
}