namespace RpgBot.Level.Abstraction
{
    public interface IRate
    {
        public float Scale { get; set; }
        public float XpBase { get; set; }
        public int StaminaPerLevel { get; set; }
        public int ManaPerLevel { get; set; }
        public int HealthPerLevel { get; set; }
        public int RegeneratePerMessages { get; set; }
        public int ManaRegen { get; set; }
        public int StaminaRegen { get; set; }
        public int HealthRegen { get; set; }
        public int ExpPerMessage { get; set; }
        public int ExpPerSticker { get; set; }
        public int ExpPerMedia { get; set; }
        public int ReputationPerPraise { get; set; }
        public int PraiseManaCost { get; set; }
        public int ReputationPerPunish { get; set; }
        public int PunishStaminaCost { get; set; }
    }
}