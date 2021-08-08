using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Service;

namespace RpgBotUnitTests.Service
{
    [TestFixture]
    public class CommandAliasServiceTests
    {
        private BotContext _context;
        private CommandAliasService _service;
        private int _contextLength;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase("bot")
                .Options;

            _context = new BotContext(options);
            _context.Database.EnsureDeleted();

            _context.CommandAliases.Add(new CommandAlias() { Id = 1, Alias = "alias1" });
            _context.CommandAliases.Add(new CommandAlias() { Id = 2, Alias = "alias2" });
            _context.CommandAliases.Add(new CommandAlias() { Id = 3, Alias = "alias3" });
            _context.SaveChanges();

            _contextLength = 3;

            _service = new CommandAliasService(_context);
        }

        [Test]
        [TestCase("alias1", "alias1", false)]
        [TestCase("alias4", null, true)]
        public void GetWillReturnCommandAlias(string alias, string actualAlias, bool isNull)
        {
            // act
            var actual = _service.Get(alias);

            // assert
            if (isNull)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.NotNull(actual);
            Assert.IsInstanceOf<CommandAlias>(actual);
            Assert.AreEqual(alias, actual.Alias);
        }

        [Test]
        [TestCase("alias1", false)]
        [TestCase("alias4", true)]
        public void DeleteWillRemoveCommandAliasOrThrowExceptionOnNull(string alias, bool itWillThrow)
        {
            // assert
            if (itWillThrow)
            {
                Assert.Throws<NotFoundException>(() => _service.Delete(alias));

                return;
            }

            _service.Delete(alias);
            Assert.IsNull(_context.CommandAliases.FirstOrDefault(c => c.Alias == alias));
        }

        [Test]
        public void ListWillReturnListOfAliases()
        {
            // act
            var actual = _service.List();
            var commandAliases = actual as CommandAlias[] ?? actual.ToArray();

            // assert
            Assert.AreEqual(commandAliases[0].Id, 1);
            Assert.AreEqual(commandAliases.Length, _contextLength);
        }

        [Test]
        [TestCase("alias5", false)]
        [TestCase("alias1", true)]
        public void CreateWillAddNewAliasOrThrowsExceptionIfAliasAlreadyExists(string alias, bool itWillThrow)
        {
            if (itWillThrow)
            {
                Assert.Throws<CommandAliasAlreadyExistsException>(() => _service.Create("alias1", "praise"));

                return;
            }

            var actual = _service.Create("alias5", "praise");

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<CommandAlias>(actual);
            Assert.AreEqual(_context.CommandAliases.First(c => c.Alias == alias), actual);
        }
    }
}