using RpgBot.Entity;
using RpgBot.Type;

namespace RpgBot.Service.Abstraction
{
    public interface IExperienceService
    {
        public User Praise(string username, User user);
        public User Punish(string username, User user);
        public User Regenerate(User user);
        public User AddExpForMessage(User user, MessageType type);
    }
}