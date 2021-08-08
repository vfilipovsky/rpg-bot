using Moq;
using NUnit.Framework;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Level.Abstraction;
using RpgBot.Service;
using RpgBot.Service.Abstraction;
using RpgBot.Type;

namespace RpgBotUnitTests.Service
{
    [TestFixture]
    public class ExperienceServiceTests
    {
        private Mock<ILevelSystem> _mockLevelSystem;
        private Mock<IUserService> _mockUserService;
        private Mock<IRate> _mockRate;

        private User _user1;

        [SetUp]
        public void SetUp()
        {
            _mockLevelSystem = new Mock<ILevelSystem>();
            _mockUserService = new Mock<IUserService>();
            _mockRate = new Mock<IRate>();

            _mockRate
                .Setup(r => r.HealthRegen)
                .Returns(5);

            _mockRate
                .Setup(r => r.ManaRegen)
                .Returns(5);

            _mockRate
                .Setup(r => r.StaminaRegen)
                .Returns(5);

            _mockRate
                .Setup(r => r.PunishStaminaCost)
                .Returns(25);

            _mockRate
                .Setup(r => r.PraiseManaCost)
                .Returns(25);

            _mockRate
                .Setup(r => r.ReputationPerPraise)
                .Returns(1);

            _mockRate
                .Setup(r => r.ReputationPerPunish)
                .Returns(-1);

            _user1 = new User()
            {
                Id = 1,
                Username = "username",
                Reputation = 1,
                ManaPoints = 100,
                MaxManaPoints = 125,
                HealthPoints = 125,
                MaxHealthPoints = 125,
                StaminaPoints = 120,
                MaxStaminaPoints = 125,
            };
        }

        [Test]
        public void RegenerateWillNotRefillIfStatReachedHisMaxValue()
        {
            // arrange
            var experienceService =
                new ExperienceService(_mockLevelSystem.Object, _mockUserService.Object, _mockRate.Object);

            // act
            var actual = experienceService.Regenerate(_user1);

            // assert
            Assert.That(actual.HealthPoints, Is.EqualTo(125));
        }

        [Test]
        public void RegenerateWillRefillStatsToUserByRates()
        {
            // arrange
            var experienceService =
                new ExperienceService(_mockLevelSystem.Object, _mockUserService.Object, _mockRate.Object);

            _user1.HealthPoints = 120;

            // act
            var actual = experienceService.Regenerate(_user1);

            // assert
            Assert.That(actual.ManaPoints, Is.EqualTo(105));
            Assert.That(actual.HealthPoints, Is.EqualTo(125));
            Assert.That(actual.StaminaPoints, Is.EqualTo(125));
        }

        [Test]
        [TestCase(23, false)]
        [TestCase(24, true)]
        public void AddExpForMessageWillIncrementsUserMessagesAndRegenerateStatsIfReachedNeededMessageCount(
            int messageCount,
            bool willRegenerate)
        {
            // arrange
            _mockUserService
                .Setup(u => u.Update(_user1))
                .Returns(_user1);

            _mockLevelSystem
                .Setup(l => l.AddExp(_user1, MessageType.Text))
                .Returns(_user1);

            _mockRate
                .Setup(r => r.RegeneratePerMessages)
                .Returns(25);

            _user1.MessagesCount = messageCount;

            var experienceService =
                new ExperienceService(_mockLevelSystem.Object, _mockUserService.Object, _mockRate.Object);

            _user1.HealthPoints = 100;
            _user1.ManaPoints = 105;
            _user1.StaminaPoints = 95;

            // act
            var actual = experienceService.AddExpForMessage(_user1, MessageType.Text);

            // assert
            Assert.NotNull(actual);
            Assert.IsInstanceOf<User>(actual);

            if (willRegenerate)
            {
                Assert.That(actual.MessagesCount, Is.EqualTo(25));
                Assert.That(actual.HealthPoints, Is.EqualTo(105));
                Assert.That(actual.ManaPoints, Is.EqualTo(110));
                Assert.That(actual.StaminaPoints, Is.EqualTo(100));
                return;
            }

            Assert.That(actual.MessagesCount, Is.EqualTo(24));
            Assert.That(actual.HealthPoints, Is.EqualTo(100));
            Assert.That(actual.ManaPoints, Is.EqualTo(105));
            Assert.That(actual.StaminaPoints, Is.EqualTo(95));
        }

        [Test]
        [TestCase("notExists", 25, false, true)]
        [TestCase("username", 20, true, false)]
        [TestCase("username", 25, false, false)]
        public void PraiseWillAddsReputationToUserAndSubtractsManaFromUserWhoRanCommand(
            string username,
            int manaPoints,
            bool notEnoughMana,
            bool userNotFound)
        {
            // arrange
            _mockUserService
                .Setup(u => u.GetByUsername(username))
                .Returns(userNotFound ? null : _user1);
            
            _mockUserService
                .Setup(u => u.Update(_user1))
                .Returns(_user1);

            _user1.ManaPoints = manaPoints;
            
            var service = new ExperienceService(_mockLevelSystem.Object, _mockUserService.Object, _mockRate.Object);

            // act
            if (notEnoughMana)
            {
                // assert
                Assert.Throws<NotEnoughManaException>(() => service.Praise("username", _user1));
                return;
            }

            if (userNotFound)
            {
                // assert
                Assert.Throws<NotFoundException>(() => service.Praise("notExists", _user1));
                return;
            }

            // act
            var actualPraisedUser = service.Praise("username", _user1);

            // assert
            Assert.NotNull(actualPraisedUser);
            Assert.IsInstanceOf<User>(actualPraisedUser);
            Assert.That(_user1.ManaPoints, Is.EqualTo(0));
            Assert.That(_user1.Reputation, Is.EqualTo(2));
        }

        [Test]
        [TestCase("notExists", 25, false, true)]
        [TestCase("username", 20, true, false)]
        [TestCase("username", 25, false, false)]
        public void PunishWillSubtractsReputationFromUserAndSubtractsStaminaFromUserWhoRanCommand(
            string username,
            int staminaPoints,
            bool notEnoughStamina,
            bool userNotFound)
        {
            // arrange
            _mockUserService
                .Setup(u => u.GetByUsername(username))
                .Returns(userNotFound ? null : _user1);
            
            _mockUserService
                .Setup(u => u.Update(_user1))
                .Returns(_user1);

            _user1.StaminaPoints = staminaPoints;
            
            var service = new ExperienceService(_mockLevelSystem.Object, _mockUserService.Object, _mockRate.Object);

            // act
            if (notEnoughStamina)
            {
                // assert
                Assert.Throws<NotEnoughStaminaException>(() => service.Punish("username", _user1));
                return;
            }

            if (userNotFound)
            {
                // assert
                Assert.Throws<NotFoundException>(() => service.Punish("notExists", _user1));
                return;
            }

            // act
            var actualPunishedUser = service.Punish("username", _user1);

            // assert
            Assert.NotNull(actualPunishedUser);
            Assert.IsInstanceOf<User>(actualPunishedUser);
            Assert.That(_user1.StaminaPoints, Is.EqualTo(0));
            Assert.That(_user1.Reputation, Is.EqualTo(0));
        }
    }
}