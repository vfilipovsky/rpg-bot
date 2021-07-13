using RpgBot.Entity;

namespace RpgBot.Service.Abstraction
{
    public interface IGroupService
    {
        public Group Get(string groupId);
    }
}