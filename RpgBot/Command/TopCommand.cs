using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class TopCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const int LevelFrom = 1;
        private const string Name = "/top";
        private const string Description = "Top players list";

        private readonly IUserService _userService;
        private readonly ILevelSystem _levelSystem;

        public TopCommand(IUserService userService, ILevelSystem levelSystem)
        {
            _userService = userService;
            _levelSystem = levelSystem;
        }
        
        public string Run(string text, User user)
        {
            var users = _userService.GetTopPlayers(user.GroupId);

            var result = "";
            var counter = 1;
            
            foreach (var u in users)
            {
                result +=
                    $"| №{counter} | {u.Username} | Lv. {u.Level} | " +
                    $"Exp: {u.Experience}/{_levelSystem.GetExpToNextLevel(u.Level)} | " +
                    $"Rep: {u.Reputation} | " +
                    $"Msg: {u.MessagesCount} |\n\n";

                counter++;
            }

            return result;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return Description;
        }

        public int GetArgsCount()
        {
            return ArgsCount;
        }

        public int GetLevelFrom()
        {
            return LevelFrom;
        }
    }
}