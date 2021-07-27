using RpgBot.Level.Abstraction;

namespace RpgBot.Level
{
    public class Rate : IRate
    {
        // Experience
        public float Scale => 1.1f;
        public float XpBase => 100.0f;
        public int StaminaPerLevel => 10;
        public int ManaPerLevel => 10;
        public int HealthPerLevel => 10;

        // Regeneration
        public int RegeneratePerMessages => 25;
        public int ManaRegen => 5;
        public int StaminaRegen => 5;
        public int HealthRegen => 5;

        // Message
        public int ExpPerMessage => 1;
        public int ExpPerSticker => 2;
        public int ExpPerMedia => 2;

        // Praise
        public int ReputationPerPraise => 1;
        public int PraiseManaCost => 25;

        // Punish
        public int ReputationPerPunish => -1;
        public int PunishStaminaCost => 25;
    }
}