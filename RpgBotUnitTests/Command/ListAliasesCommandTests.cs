using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class ListAliasesCommandTests
    {
        [Test]
        public void ListWillReturnListOfAliases()
        {
            // arrange
            var mockCommandAliasService = new Mock<ICommandAliasService>();
            mockCommandAliasService
                .Setup(a => a.List())
                .Returns(new List<CommandAlias>()
                {
                    new() {Alias = "alias1", Name = "praise"},
                    new() {Alias = "alias2", Name = "punish"}
                });

            var command = new ListAliasesCommand(mockCommandAliasService.Object);
            var expected =
                $"alias1 -> praise\n" +
                $"alias2 -> punish";

            // act
            var actual = command.Run("", new User());

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new ListAliasesCommand(new Mock<ICommandAliasService>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/aliases", command.Name);
                Assert.AreEqual("Lists all aliases", command.Description);
                Assert.AreEqual(0, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }
}