namespace RpgBot.Level.Abstraction
{
    public interface IRate
    {
        public float Scale { get; }
        public float XpBase { get; }
        public int StaminaPerLevel { get; }
        public int ManaPerLevel { get; }
        public int HealthPerLevel { get; }
        public int RegeneratePerMessages { get; }
        public int ManaRegen { get; }
        public int StaminaRegen { get; }
        public int HealthRegen { get; }
        public int ExpPerMessage { get; }
        public int ExpPerSticker { get; }
        public int ExpPerMedia { get; }
        public int ReputationPerPraise { get; }
        public int PraiseManaCost { get; }
        public int ReputationPerPunish { get; }
        public int PunishStaminaCost { get; }
    }
}