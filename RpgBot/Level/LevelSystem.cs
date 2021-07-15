using System;
using RpgBot.Entity;

namespace RpgBot.Level
{
    public static class LevelSystem
    {
        public static User AddExp(User user, int exp)
        {
            user.Experience += exp;
            var experienceToNextLevel = GetExpToNextLevel(user.Level);

            if (user.Experience < experienceToNextLevel) return user;
            
            user.Level += 1;
            user.MaxHealthPoints += Rate.HealthPerLevel;
            user.MaxStaminaPoints += Rate.StaminaPerLevel;
            user.MaxManaPoints += Rate.ManaPerLevel;
            
            user.HealthPoints = user.MaxHealthPoints;
            user.ManaPoints = user.MaxManaPoints;
            user.StaminaPoints = user.MaxStaminaPoints;
            
            user.Experience -= experienceToNextLevel;

            return user;
        }

        public static int GetExpToNextLevel(int currentLevel)
        {
            return (int)(Math.Pow(Rate.XpBase * currentLevel, Rate.Scale));
        }
    }
}