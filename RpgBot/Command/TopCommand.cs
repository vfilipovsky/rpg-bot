using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class TopCommand : AbstractCommand, ICommand
    {
        private const int ArgsCount = 0;
        private const string Name = "/top";
        private const string Description = "List of top players";

        private readonly IUserService _userService;

        public TopCommand(IUserService userService)
        {
            _userService = userService;
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
                    $"Exp: {u.Experience}/{LevelSystem.GetExpToNextLevel(u.Level)} | " +
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
    }
}