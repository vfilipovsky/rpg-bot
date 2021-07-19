using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBot.Command
{
    public class TopCommand : AbstractCommand, ICommand
    {
        public string Name { get; set; } = "/top";
        public string Description { get; set; } = "Top players list";
        public int ArgsCount { get; set; } = 0;
        public int LevelFrom { get; set; } = 1;

        private readonly IUserService _userService;
        private readonly ILevelSystem _levelSystem;

        public TopCommand(IUserService userService, ILevelSystem levelSystem)
        {
            _userService = userService;
            _levelSystem = levelSystem;
        }

        public string Run(string text, User user)
        {
            var users = _userService.GetTopPlayers();

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
    }
}