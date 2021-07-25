using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Entity;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class AboutCommandTests
    {
        private Mock<IUserService> _mockUserService;
        private User _user;
        private AboutCommand _command;
        private string _message;

        [SetUp]
        public void SetUp()
        {
            _user = new User() {Username = "username", UserId = "123"};
            _message = "/about @username";
        }

        [Test]
        public void RunCommandWillGetsUserFromUserServiceAndReturnsItAsString()
        {
            // arrange
            _mockUserService = new Mock<IUserService>();
            _mockUserService
                .Setup(u => u.Stringify(_user))
                .Returns(
                    $"Id: {_user.UserId}\n" +
                    $"Name: {_user.Username}\n" +
                    $"Msg: {_user.MessagesCount}\n" +
                    $"Rep: {_user.Reputation}\n" +
                    $"LVL: {_user.Level}\n" +
                    $"Exp: {_user.Experience}/158\n" +
                    $"HP: {_user.HealthPoints}/{_user.MaxHealthPoints}\n" +
                    $"MP: {_user.ManaPoints}/{_user.MaxManaPoints}\n" +
                    $"SP: {_user.StaminaPoints}/{_user.MaxStaminaPoints}");
            
            _mockUserService
                .Setup(u => u.GetByUsername("username"))
                .Returns(_user);
            
            _command = new AboutCommand(_mockUserService.Object);

            var expected =
                $"Id: {_user.UserId}\n" +
                $"Name: {_user.Username}\n" +
                $"Msg: {_user.MessagesCount}\n" +
                $"Rep: {_user.Reputation}\n" +
                $"LVL: {_user.Level}\n" +
                $"Exp: {_user.Experience}/158\n" +
                $"HP: {_user.HealthPoints}/{_user.MaxHealthPoints}\n" +
                $"MP: {_user.ManaPoints}/{_user.MaxManaPoints}\n" +
                $"SP: {_user.StaminaPoints}/{_user.MaxStaminaPoints}";

            // act
            var actual = _command.Run(_message, _user);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RunWillReturnUserNotFoundStringIfUserNotFound()
        {
            // arrange
            _mockUserService = new Mock<IUserService>();
            _mockUserService
                .Setup(u => u.GetByUsername("username"))
                .Returns((User) null);

            _command = new AboutCommand(_mockUserService.Object);

            const string expected = "User 'username' not found.";

            // act
            var actual = _command.Run(_message, _user);
            
            // assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new AboutCommand(new Mock<IUserService>().Object);
            
            Assert.Multiple(() =>
            {
                Assert.AreEqual("/about", command.Name);
                Assert.AreEqual("Show details about target user", command.Description);
                Assert.AreEqual(1, command.ArgsCount);
                Assert.AreEqual(1, command.RequiredLevel);
            });
        }
    }
}