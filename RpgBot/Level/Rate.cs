namespace RpgBot.Level
{
    public static class Rate
    {
        public const float Scale = 1.1f;
        public const float XpBase = 100.0f;

        // Message
        public const int ExpPerMessage = 1;
        public const int ExpPerImage = 1;
        
        // Praise
        public const int ReputationPerPraise = 1;
        public const int PraiseDailyMaxCount = 3;
        
        // Punish
        public const int ReputationPerPunish = -1;
        public const int PunishDailyMaxCount = 3;

    }
}