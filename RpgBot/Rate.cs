namespace RpgBot
{
    public enum Rate
    {
        Multiplier = 1,

        // Message
        ExpPerMessage = 1,
        ExpPerImage = 1,
        
        // Praise
        ReputationPerPraise = 1,
        PraiseDailyMaxCount = 3,
        
        // Punish
        ReputationPerPunish = -1,
        PunishDailyMaxCount = 3,

    }
}