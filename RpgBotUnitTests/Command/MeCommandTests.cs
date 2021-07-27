using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class MeCommandTests
    {
        private Mock<IUserService> _mockUserService;

        [Test]
        public void RunCommandWillReturnUserAsString()
        {
            // arrange
            var user = new User() {Username = "username"};
            
            _mockUserService = new Mock<IUserService>();
            _mockUserService
                .Setup(u => u.Stringify(user))
                .Returns($"Id: {user.UserId}\n" +
                         $"Name: {user.Username}\n" +
                         $"Msg: {user.MessagesCount}\n" +
                         $"Rep: {user.Reputation}\n" +
                         $"LVL: {user.Level}\n" +
                         $"Exp: {user.Experience}/158\n" +
                         $"HP: {user.HealthPoints}/{user.MaxHealthPoints}\n" +
                         $"MP: {user.ManaPoints}/{user.MaxManaPoints}\n" +
                         $"SP: {user.StaminaPoints}/{user.MaxStaminaPoints}");

            var command = new MeCommand(_mockUserService.Object);

            var expected =
                $"Id: {user.UserId}\n" +
                $"Name: {user.Username}\n" +
                $"Msg: {user.MessagesCount}\n" +
                $"Rep: {user.Reputation}\n" +
                $"LVL: {user.Level}\n" +
                $"Exp: {user.Experience}/158\n" +
                $"HP: {user.HealthPoints}/{user.MaxHealthPoints}\n" +
                $"MP: {user.ManaPoints}/{user.MaxManaPoints}\n" +
                $"SP: {user.StaminaPoints}/{user.MaxStaminaPoints}";

            // act
            var actual = command.Run("", user);

            // assert
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new MeCommand(new Mock<IUserService>().Object);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual("/me", command.Name);
                Assert.AreEqual("Show details about you", command.Description);
                Assert.AreEqual(0, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }
}