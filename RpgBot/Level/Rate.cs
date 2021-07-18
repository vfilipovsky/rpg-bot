using RpgBot.Level.Abstraction;

namespace RpgBot.Level
{
    public class Rate : IRate
    {
        // Experience
        public float Scale { get; set; } = 1.1f;
        public float XpBase { get; set; } = 100.0f;
        public int StaminaPerLevel { get; set; } = 10;
        public int ManaPerLevel { get; set; } =  10;
        public int HealthPerLevel { get; set; } =  10;

        // Regeneration
        public int RegeneratePerMessages { get; set; } =  25;
        public int ManaRegen { get; set; } =  5;
        public int StaminaRegen { get; set; } =  5;
        public int HealthRegen { get; set; } =  5;
        
        // Message
        public int ExpPerMessage { get; set; } =  1;
        
        // Praise
        public int ReputationPerPraise { get; set; } =  1;
        public int PraiseManaCost { get; set; } =  25;
        
        // Punish
        public int ReputationPerPunish { get; set; } =  -1;
        public int PunishStaminaCost { get; set; } =  25;
    }
}