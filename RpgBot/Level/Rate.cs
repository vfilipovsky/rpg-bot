namespace RpgBot.Level
{
    public static class Rate
    {
        // Experience
        public const float Scale = 1.1f;
        public const float XpBase = 100.0f;
        public const int StaminaPerLevel = 10;
        public const int ManaPerLevel = 10;
        public const int HealthPerLevel = 10;

        // Regeneration
        public const int RegeneratePerMessages = 25;
        public const int ManaRegen = 5;
        public const int StaminaRegen = 5;
        public const int HealthRegen = 5;
        
        // Message
        public const int ExpPerMessage = 1;
        public const int ExpPerImage = 1;
        
        // Praise
        public const int ReputationPerPraise = 1;
        public const int PraiseManaCost = 25;
        
        // Punish
        public const int ReputationPerPunish = -1;
        public const int PunishStaminaCost = 25;

    }
}