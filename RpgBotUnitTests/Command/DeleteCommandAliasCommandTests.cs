using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class DeleteCommandAliasCommandTests
    {
        private Mock<ICommandAliasService> _mockCommandAliasService;
        private Mock<ICommandArgsResolver> _mockCommandArgsResolver;

        [Test]
        public void DeleteCommandWillRemovesAlias()
        {
            // arrange
            _mockCommandArgsResolver = new Mock<ICommandArgsResolver>();
            _mockCommandArgsResolver
                .Setup(a => a.GetArgs("/dalias alias", 1))
                .Returns(new[] {"/dalias", "alias"});

            _mockCommandAliasService = new Mock<ICommandAliasService>();
            _mockCommandAliasService
                .Setup(a => a.Delete("alias"))
                .Returns(new CommandAlias());

            var command =
                new DeleteCommandAliasCommand(_mockCommandAliasService.Object, _mockCommandArgsResolver.Object);
            
            // act
            var actual = command.Run("/dalias alias", new User());

            // assert
            Assert.AreEqual("Alias successfully removed", actual);
        }
        
        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new DeleteCommandAliasCommand(
                new Mock<ICommandAliasService>().Object,
                new Mock<ICommandArgsResolver>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/dalias", command.Name);
                Assert.AreEqual("Remove alias", command.Description);
                Assert.AreEqual(1, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }

}