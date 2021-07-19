using System.ComponentModel.DataAnnotations.Schema;

namespace RpgBot.Entity
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
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
    }
}