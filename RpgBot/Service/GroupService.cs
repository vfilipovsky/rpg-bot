using System.Linq;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBot.Service
{
    public class GroupService : IGroupService
    {
        private readonly BotContext _botContext;

        public GroupService(BotContext botContext)
        {
            _botContext = botContext;
        }

        public Group Get(string groupId)
        {
            var exists = _botContext.Groups.FirstOrDefault(g => g.Id == groupId);

            if (null != exists) return exists;

            var group = new Group() {Id = groupId};
            _botContext.Groups.Add(group);
            _botContext.SaveChanges();

            return group;
        }
    }
}