using System;
using RpgBot.Entity;

namespace RpgBot.Level
{
    public static class LevelSystem
    {
        public static User AddExp(User user, int exp)
        {
            user.Experience += exp;

            return user.Experience < GetExpToNextLevel(user.Level) ? user : LevelUp(user);
        }

        public static User LevelUp(User user)
        {
            user.Level += 1;
            user.MaxHealthPoints += Rate.HealthPerLevel;
            user.MaxStaminaPoints += Rate.StaminaPerLevel;
            user.MaxManaPoints += Rate.ManaPerLevel;
            
            user.HealthPoints = user.MaxHealthPoints;
            user.ManaPoints = user.MaxManaPoints;
            user.StaminaPoints = user.MaxStaminaPoints;
            
            user.Experience = 0;

            return user;
        }
        
        public static int GetExpToNextLevel(int currentLevel)
        {
            return (int)(Math.Pow(Rate.XpBase * currentLevel, Rate.Scale));
        }
    }
}