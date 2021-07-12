using System.Collections.Generic;

namespace RpgBot.Entity
{
    public class Group
    {
        public string Id { get; set; }
        public List<User> Users { get; set; }
    }
}