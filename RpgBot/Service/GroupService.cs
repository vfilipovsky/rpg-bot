using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;

namespace RpgBot.Service
{
    public class GroupService
    {
        private readonly BotContext _context;

        public GroupService()
        {
            _context = new BotContext();
        }

        public Group Get(string groupId)
        {
            var exists = _context.Groups.FirstOrDefault(g => g.Id == groupId);

            if (null != exists) return exists;

            var group = new Group() {Id = groupId};
            _context.Groups.Add(group);
            _context.SaveChanges();

            return group;
        }
    }
}