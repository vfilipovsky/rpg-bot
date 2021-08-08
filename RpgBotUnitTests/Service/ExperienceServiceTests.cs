using Moq;
using NUnit.Framework;
using RpgBot.Entity;
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
            
            _user1 = new User()
            {
                Id = 1,
                Username = "username",
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
    }
}