using System;
using RpgBot.Entity;

namespace RpgBot.Level
{
    public static class LevelSystem
    {
        public static User AddExp(User user, int exp)
        {
            user.Experience += exp;
            // todo: use rates from enum
            var experienceToNextLevel = GetExpToNextLevel(user.Level);

            if (user.Experience <= experienceToNextLevel) return user;
            
            user.Level += 1;
            user.Experience -= experienceToNextLevel;

            return user;
        }

        public static int GetExpToNextLevel(int currentLevel)
        {
            return (int)(Math.Pow(Rate.XpBase * currentLevel, Rate.Scale));
        }
    }
}