using Moq;
using NUnit.Framework;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Level.Abstraction;
using RpgBot.Type;

namespace RpgBotUnitTests.Level
{
    [TestFixture]
    public class LevelSystemTests
    {
        private User _user;
        private Mock<IRate> _mockRate;
        private ILevelSystem _levelSystem;

        [SetUp]
        public void SetUp()
        {
            _mockRate = new Mock<IRate>();
            _mockRate.Setup(r => r.Scale).Returns(1.1f);
            _mockRate.Setup(r => r.XpBase).Returns(100.0f);

            _levelSystem = new LevelSystem(_mockRate.Object);
            _user = new User() {Level = 1, Experience = 1};
        }

        [Test]
        public void LevelUpShouldIncreaseUserLevelByOneAndSetExperienceToZero()
        {
            // act
            _levelSystem.LevelUp(_user);

            // assert
            Assert.Multiple(() =>
            {
                Assert.That(_user.Level, Is.EqualTo(2));
                Assert.That(_user.Experience, Is.EqualTo(0));
            });
        }

        [Test]
        public void GetExpToNextLevelShouldReturnACalculatedValueFromFormula()
        {
            // arrange
            const int getExpFormulaResult = 158;

            // act
            var exp = _levelSystem.GetExpToNextLevel(_user.Level);

            // assert
            Assert.That(exp, Is.EqualTo(getExpFormulaResult));
        }

        [Test]
        public void AddExpShouldIncrementUsersExpByMessageTypeText()
        {
            // arrange
            _mockRate.Setup(r => r.ExpPerMessage).Returns(1);

            // act
            _levelSystem.AddExp(_user, MessageType.Text);

            // assert
            Assert.That(_user.Experience, Is.EqualTo(2));
        }
        
        [Test]
        public void AddExpShouldIncrementUsersExpByMessageTypeSticker()
        {
            // arrange
            _mockRate.Setup(r => r.ExpPerSticker).Returns(3);
            
            // act
            _levelSystem.AddExp(_user, MessageType.Sticker);

            // assert
            Assert.That(_user.Experience, Is.EqualTo(4));
        }
        
        [Test]
        public void AddExpShouldIncrementUsersExpByMessageTypeImage()
        {
            // arrange
            _mockRate.Setup(r => r.ExpPerMedia).Returns(2);
            
            // act
            _levelSystem.AddExp(_user, MessageType.Image);

            // assert
            Assert.That(_user.Experience, Is.EqualTo(3));
        }

    }
}