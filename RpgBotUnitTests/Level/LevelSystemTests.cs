using Moq;
using NUnit.Framework;
using RpgBot.Entity;
using RpgBot.Level;
using RpgBot.Level.Abstraction;

namespace RpgBotUnitTests.Level
{
    [TestFixture]
    public class LevelSystemTests
    {
        private User _user;
        private Mock<IRate> _mockRate;
        private ILevelSystem _levelSystem;
        private int _getExpFormulaResult;

        [SetUp]
        public void SetUp()
        {
            _mockRate = new Mock<IRate>();
            _mockRate.Setup(r => r.Scale).Returns(1.1f);
            _mockRate.Setup(r => r.XpBase).Returns(100.0f);
            _mockRate.Setup(r => r.ExpPerMessage).Returns(1);
            _getExpFormulaResult = 158;

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
            // act
            var exp = _levelSystem.GetExpToNextLevel(_user.Level);

            // assert
            Assert.That(exp, Is.EqualTo(_getExpFormulaResult));
        }

        [Test]
        public void AddExpShouldIncrementUsersExpByGivenRate()
        {
            // act
            _levelSystem.AddExp(_user, 1);

            // assert
            Assert.That(_user.Experience, Is.EqualTo(2));
        }
    }
}