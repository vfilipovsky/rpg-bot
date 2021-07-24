using System;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Type;

namespace RpgBot.Level
{
    public class LevelSystem : ILevelSystem
    {
        private readonly IRate _rate;

        public LevelSystem(IRate rate)
        {
            _rate = rate;
        }
        
        public User AddExp(User user, MessageType type)
        {
            var exp = type switch
            {
                MessageType.Image => _rate.ExpPerMedia,
                MessageType.Sticker => _rate.ExpPerSticker,
                MessageType.Text => _rate.ExpPerMessage,
                _ => _rate.ExpPerMessage
            };

            user.Experience += exp;

            return user.Experience < GetExpToNextLevel(user.Level) ? user : LevelUp(user);
        }

        public User LevelUp(User user)
        {
            user.Level += 1;
            user.MaxHealthPoints += _rate.HealthPerLevel;
            user.MaxStaminaPoints += _rate.StaminaPerLevel;
            user.MaxManaPoints += _rate.ManaPerLevel;
            
            user.HealthPoints = user.MaxHealthPoints;
            user.ManaPoints = user.MaxManaPoints;
            user.StaminaPoints = user.MaxStaminaPoints;
            
            user.Experience = 0;

            return user;
        }
        
        public int GetExpToNextLevel(int currentLevel)
        {
            return (int)(Math.Pow(_rate.XpBase * currentLevel, _rate.Scale));
        }
    }
}