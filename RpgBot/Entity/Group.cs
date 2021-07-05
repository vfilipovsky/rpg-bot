using System.Collections.Generic;

namespace RpgBot.Entity
{
    public class Group
    {
        public int GroupId { get; set; }
        public List<User> Users { get; set; }
    }
}