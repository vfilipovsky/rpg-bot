using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class PraiseCommandTests
    {
        private Mock<IExperienceService> _mockExperienceService;
        private Mock<IRate> _mockRate;
        private Mock<ICommandArgsResolver> _mockCommandArgsResolver;

        private const int PraiseManaCost = 25;

        [SetUp]
        public void SetUp()
        {
            _mockCommandArgsResolver = new Mock<ICommandArgsResolver>();
            _mockExperienceService = new Mock<IExperienceService>();
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
        public void RunPraiseCommandWillAddsReputationToUser()
        {
            // arrange
            var user = new User() { Username = "username2", ManaPoints = 25 };

            _mockExperienceService
                .Setup(u => u.Praise("username", user))
                .Returns(new User() { Username = "username", Reputation = 1 });

            var command = new PraiseCommand(_mockExperienceService.Object, _mockCommandArgsResolver.Object);

            // act
            var actual = command.Run($"/praise @username", user);

            // asserts
            Assert.AreEqual("username got praised. 1 reputation in total.", actual);
        }

        [Test]
        public void RunPraiseCommandWillThrowPraiseYourselfExceptionIfUsersTriesPraiseHisSelf()
        {
            // arrange 
            var command = new PraiseCommand(_mockExperienceService.Object, _mockCommandArgsResolver.Object);
            var user = new User() { Username = "username" };
            
            // assert
            Assert.Throws<PraiseYourselfException>(() => command.Run("/praise @username", user));
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new PraiseCommand(
                new Mock<IExperienceService>().Object,
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