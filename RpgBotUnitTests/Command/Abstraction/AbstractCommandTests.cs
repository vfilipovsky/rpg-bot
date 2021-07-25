using System.Linq;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Exception;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command.Abstraction
{
    [TestFixture]
    public class AbstractCommandTests
    {
        private Mock<IUserService> _mockUserService;
        private AboutCommand _command;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _command = new AboutCommand(_mockUserService.Object);
        }

        [Test]
        public void GetArgsWillReturnListOfMessageSplitBySpace()
        {
            // arrange
            const string message = "/about @Username";

            // act
            var args = _command.GetArgs(message, _command.ArgsCount);

            // assert
            var enumerable = args.ToList();
            Assert.That(enumerable.Count - 1, Is.EqualTo(_command.ArgsCount)); // ArgsCount excludes commandName
            Assert.That(enumerable.ElementAt(1), Is.EqualTo("@Username"));
        }

        [Test]
        public void GetArgsWillThrowExceptionIfPartsLengthLessThatArgsCount()
        {
            // arrange
            const string message = "/about";
            var mockAbstractCommand = new Mock<AbstractCommand>() {CallBase = true};
            mockAbstractCommand
                .Setup(a => a.GetArgs(message, 1))
                .Throws(new BotException("Pass the argument to command"));

            // assert
            Assert.Throws<BotException>(
                () => _command.GetArgs(message, 1),
                "Pass the argument to command");
        }

        [Test]
        public void GetArgsWillClearNewLineSymbolInMessage()
        {
            // arrange
            var parts = new[]
            {
                "/about\n",
                "@username\n"
            };

            var expectedParts = new[]
            {
                "/about",
                "@username"
            };

            // act
            var actualParts = _command.ClearArgs(parts, 2);

            // assert
            Assert.AreEqual(expectedParts, actualParts);
        }
    }
}