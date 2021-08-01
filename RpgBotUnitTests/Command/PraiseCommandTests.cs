using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class PraiseCommandTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IRate> _mockRate;
        private Mock<ICommandArgsResolver> _mockCommandArgsResolver;

        private const int PraiseManaCost = 25;

        [SetUp]
        public void SetUp()
        {
            _mockCommandArgsResolver = new Mock<ICommandArgsResolver>();
            _mockUserService = new Mock<IUserService>();
            _mockRate = new Mock<IRate>();

            _mockCommandArgsResolver
                .Setup(c => c.GetArgs("/praise @username", 1))
                .Returns(new List<string>()
                {
                    "/praise",
                    "@username"
                });

            _mockRate
                .Setup(r => r.PraiseManaCost)
                .Returns(PraiseManaCost);
        }

        [Test]
        public void RunPraiseCommandWillFailIfUserNotFound()
        {
            // arrange
            var user = new User() {Username = "username2", ManaPoints = 25};

            _mockUserService
                .Setup(u => u.Praise("username", user))
                .Returns((User) null);

            var command = new PraiseCommand(_mockUserService.Object, _mockRate.Object, _mockCommandArgsResolver.Object);

            // act
            var actual = command.Run($"/praise @username", user);

            // asserts
            Assert.AreEqual("User 'username' not found", actual);
        }

        [Test]
        public void RunPraiseCommandWillAddsReputationToUserIfFound()
        {
            // arrange
            var user = new User() {Username = "username2", ManaPoints = 25};

            _mockUserService
                .Setup(u => u.Praise("username", user))
                .Returns(new User() {Username = "username", Reputation = 1});

            var command = new PraiseCommand(_mockUserService.Object, _mockRate.Object, _mockCommandArgsResolver.Object);

            // act
            var actual = command.Run($"/praise @username", user);

            // asserts
            Assert.AreEqual("username got praised. 1 reputation in total.", actual);
        }

        [Test]
        public void RunPraiseCommandCannotSucceedWhenNotEnoughMana()
        {
            // arrange
            var command = new PraiseCommand(_mockUserService.Object, _mockRate.Object, _mockCommandArgsResolver.Object);
            var user = new User() {Username = "username2", ManaPoints = 20};

            // act
            var actual = command.Run($"/praise @username", user);

            // asserts
            Assert.AreEqual($"Not enough mana, need {PraiseManaCost} (20).", actual);
        }

        [Test]
        public void RunPraiseCommandIsNotAllowedToPraiseYourself()
        {
            // arrange
            var command = new PraiseCommand(_mockUserService.Object, _mockRate.Object, _mockCommandArgsResolver.Object);

            // act
            var actual = command.Run($"/praise @username", new User() {Username = "username"});

            // asserts
            Assert.AreEqual("You cannot praise yourself", actual);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new PraiseCommand(
                new Mock<IUserService>().Object,
                new Mock<IRate>().Object,
                new Mock<ICommandArgsResolver>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/praise", command.Name);
                Assert.AreEqual("Praise user and give him +1 to reputation.", command.Description);
                Assert.AreEqual(1, command.ArgsCount);
                Assert.AreEqual(2, command.RequiredLevel);
            });
        }
    }
}