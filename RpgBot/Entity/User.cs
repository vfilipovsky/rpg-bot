namespace RpgBot.Entity
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Reputation { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Level { get; set; } = 1;
        public int HealthPoints { get; set; } = 100;
        public int MaxHealthPoints { get; set; } = 100;
        public int ManaPoints { get; set; } = 100;
        public int MaxManaPoints { get; set; } = 100;
        public int StaminaPoints { get; set; } = 100;
        public int MaxStaminaPoints { get; set; } = 100;
        public int MessagesCount { get; set; } = 0;
        public Group Group { get; set; }
    }
}