using RpgBot.Entity;
using RpgBot.Type;

namespace RpgBot.Level.Abstraction
{
    public interface ILevelSystem
    {
        public int GetExpToNextLevel(int currentLevel);
        public User LevelUp(User user);
        public User AddExp(User user, MessageType type);
    }
}