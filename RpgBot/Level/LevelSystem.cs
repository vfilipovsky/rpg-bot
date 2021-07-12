using System;
using RpgBot.Entity;

namespace RpgBot.Level
{
    public static class LevelSystem
    {
        public static User AddExp(User user, Rate rate)
        {
            user.Experience += (int)rate;
            // todo: use rates from enum
            var experienceToNextLevel = (int)(50f * (MathF.Pow(user.Level + 1, 2) - (5 * (user.Level + 1)) + 8));

            if (user.Experience <= experienceToNextLevel) return user;
            
            user.Level += 1;
            user.Experience -= experienceToNextLevel;

            return user;
        }
    }
}