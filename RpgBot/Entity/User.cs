namespace RpgBot.Entity
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public int Reputation { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Level { get; set; } = 1;
        public Group Group { get; set; }
    }
}