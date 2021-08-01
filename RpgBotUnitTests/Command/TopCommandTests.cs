using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class TopCommandTests
    {
        [Test]
        public void RunCommandWillReturnListOfUsersAsString()
        {
            // arrange
            var mockUserService = new Mock<IUserService>();
            var mockLeveSystem = new Mock<ILevelSystem>();
            const int returnsExp = 200;

            mockLeveSystem
                .Setup(l => l.GetExpToNextLevel(2))
                .Returns(returnsExp);
            
            mockUserService
                .Setup(u => u.GetTopPlayers())
                .Returns(new List<User>()
                {
                    new()
                    {
                        Id = 2, Username = "username2", Experience = 2, Level = 2, Reputation = 2, MessagesCount = 2
                    },
                    new()
                    {
                        Id = 1, Username = "username1", Experience = 1, Level = 2, Reputation = 1, MessagesCount = 1
                    }
                });

            const string expected =
                "| №1 | username2 | Lv. 2 | " +
                "Exp: 2/200 | " +
                "Rep: 2 | " +
                "Msg: 2 |\n\n" +
                "| №2 | username1 | Lv. 2 | " +
                "Exp: 1/200 | " +
                "Rep: 1 | " +
                "Msg: 1 |\n\n";
            
                var command = new TopCommand(mockUserService.Object, mockLeveSystem.Object);

            // act
            var actual = command.Run("/top", new User());

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new TopCommand(new Mock<IUserService>().Object, new Mock<ILevelSystem>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/top", command.Name);
                Assert.AreEqual("Top players list", command.Description);
                Assert.AreEqual(0, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }
}