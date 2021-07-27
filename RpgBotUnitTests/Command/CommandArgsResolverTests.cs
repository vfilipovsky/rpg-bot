using System.Linq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Exception;

namespace RpgBotUnitTests.Command
{
    [TestFixture]
    public class CommandArgsResolverTests
    {
        private CommandArgsResolver _commandArgsResolver;

        [SetUp]
        public void SetUp()
        {
            _commandArgsResolver = new CommandArgsResolver();
        }

        [Test]
        public void GetArgsWillReturnListOfMessageSplitBySpace()
        {
            // arrange
            const string message = "/about @Username";
            const int argsCount = 1;

            // act
            var args = _commandArgsResolver.GetArgs(message, argsCount);

            // assert
            var enumerable = args.ToList();
            Assert.That(enumerable.Count - 1, Is.EqualTo(argsCount)); // ArgsCount excludes commandName
            Assert.That(enumerable.ElementAt(1), Is.EqualTo("@Username"));
        }

        [Test]
        public void GetArgsWillThrowExceptionIfPartsLengthLessThatArgsCount()
        {
            // arrange
            const string message = "/about @Username";
            const int argsCount = 2;

            // assert
            Assert.Throws<BotException>(
                () => _commandArgsResolver.GetArgs(message, argsCount),
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
            var actualParts = _commandArgsResolver.ClearArgs(parts, 2);

            // assert
            Assert.AreEqual(expectedParts, actualParts);
        }
    }
}