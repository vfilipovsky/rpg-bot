using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class CreateCommandAliasCommandTests
    {
        private Mock<ICommandAliasService> _mockCommandAliasService;
        private Mock<ICommandArgsResolver> _mockCommandArgsResolver;

        [Test]
        [TestCase("++", "praise", false, false)]
        [TestCase("++", "noCommand", true, false)]
        [TestCase("praise", "punish", false, true)]
        public void RunCommandWillAddNewAliasIfPassAllChecks(
            string alias,
            string commandName,
            bool itWillThrowNotFoundException,
            bool itWillThrowAliasValidationException)
        {
            // arrange
            _mockCommandArgsResolver = new Mock<ICommandArgsResolver>();
            _mockCommandAliasService = new Mock<ICommandAliasService>();

            _mockCommandArgsResolver
                .Setup(a => a.GetArgs($"/alias {alias} {commandName}", 2))
                .Returns(new[] {"/alias", alias, commandName});

            _mockCommandAliasService
                .Setup(a => a.Create(alias, commandName))
                .Returns(new CommandAlias());

            var command =
                new CreateCommandAliasCommand(_mockCommandAliasService.Object, _mockCommandArgsResolver.Object);

            // assert
            if (itWillThrowAliasValidationException)
            {
                Assert.Throws<AliasValidationException>(
                    () => command.Run($"/alias {alias} {commandName}", new User()));
                
                return;
            }

            if (itWillThrowNotFoundException)
            {
                Assert.Throws<NotFoundException>(
                    () => command.Run($"/alias {alias} {commandName}", new User()));
                
                return;
            }

            var actual = command.Run($"/alias {alias} {commandName}", new User());
            
            Assert.AreEqual("Alias successfully created", actual);
        }

        [Test]
        [TestCase("@alias", true)]
        [TestCase("alias", false)]
        public void ValidateAliasWillThrowExceptionIfItWillContainsBannedChar(string alias, bool itWillThrow)
        {
            if (itWillThrow)
            {
                Assert.Throws<AliasValidationException>((() => CreateCommandAliasCommand.ValidateAlias(alias)));
                return;
            }

            CreateCommandAliasCommand.ValidateAlias(alias);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new CreateCommandAliasCommand(
                new Mock<ICommandAliasService>().Object,
                new Mock<ICommandArgsResolver>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/alias", command.Name);
                Assert.AreEqual("Creates alias for a command ($alias, $name)", command.Description);
                Assert.AreEqual(2, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }
}