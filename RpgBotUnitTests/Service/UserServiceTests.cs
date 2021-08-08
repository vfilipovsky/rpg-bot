using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RpgBot.Context;
using RpgBot.Entity;
using RpgBot.Level.Abstraction;
using RpgBot.Service;

namespace RpgBotUnitTests.Service
{
    [TestFixture]
    public class UserServiceTests
    {
        private BotContext _context;
        private Mock<IRate> _mockRate;
        private Mock<ILevelSystem> _mockLevelSystem;

        private User _user1;
        private User _user2;
        private User _user3;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<BotContext>()
                .UseInMemoryDatabase("bot")
                .Options;

            _context = new BotContext(options);
            _context.Database.EnsureDeleted();

            _user1 = new User() { Id = 1, UserId = "userid1", Username = "username" };
            _user2 = new User() { Id = 2, UserId = "userid2", Username = "username2" };
            _user3 = new User() { Id = 3, UserId = "userid3", Username = "username3" };

            _context.Users.Add(_user1);
            _context.Users.Add(_user2);
            _context.Users.Add(_user3);
            _context.SaveChanges();

            _mockRate = new Mock<IRate>();
            _mockLevelSystem = new Mock<ILevelSystem>();
        }

        [Test]
        public void CreateWillAddsNewUserToDatabaseAndReturnsItBack()
        {
            // arrange
            var userService = new UserService(_context, _mockRate.Object, _mockLevelSystem.Object);
            const string username = "username";
            const string userId = "userid123";

            // act
            var actual = userService.Create(username, userId);
            var savedUser = _context.Users.First(u => u.Username == username);

            // assert
            Assert.NotNull(savedUser);
            Assert.IsInstanceOf<User>(savedUser);
            Assert.AreEqual(username, actual.Username);
            Assert.AreEqual(userId, actual.UserId);
        }

        [Test]
        [TestCase("notExists", true)]
        [TestCase("username", false)]
        public void GetByUsernameWillReturnUserIfFoundsOrNull(string username, bool isNull)
        {
            // arrange
            var userService = new UserService(_context, _mockRate.Object, _mockLevelSystem.Object);

            // act
            var actual = userService.GetByUsername(username);

            // assert
            if (isNull)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<User>(actual);
            Assert.AreEqual(username, actual.Username);
        }

        [Test]
        public void UpdateWillUpdatesUserFields()
        {
            // arrange
            var userService = new UserService(_context, _mockRate.Object, _mockLevelSystem.Object);
            const int newMessagesCount = 2;

            _user1.MessagesCount = newMessagesCount;

            // act
            var actual = userService.Update(_user1);

            // assert
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<User>(actual);
            Assert.AreEqual(newMessagesCount, _context.Users.First(u => u.Id == 1).MessagesCount);
        }

        [Test]
        [TestCase("notExists", true)]
        [TestCase("userid1", false)]
        public void GetByUserIdWillReturnUserIfFoundsOrNull(string userId, bool isNull)
        {
            // arrange
            var userService = new UserService(_context, _mockRate.Object, _mockLevelSystem.Object);

            // act
            var actual = userService.GetByUserId(userId);

            // assert
            if (isNull)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf<User>(actual);
            Assert.AreEqual(userId, actual.UserId);
        }

        [Test]
        [TestCase("username", "userid1", false, false, true)]
        [TestCase("username5", "userid5", true, false, false)]
        [TestCase("username", "userid1", false, true, false)]
        [TestCase(null, "userid1", false, true, false)]
        public void GetWillGetsOrCreatesNewUserAndUpdatesItUsernameIfChanged(
            string username,
            string userId,
            bool creates,
            bool updates,
            bool gets)
        {
            // arrange
            var userService = new UserService(_context, _mockRate.Object, _mockLevelSystem.Object);
            
            if (gets)
            {
                // act
                var actual = userService.Get(username, userId);
                
                // assert
                Assert.IsNotNull(actual);
                Assert.IsInstanceOf<User>(actual);
                Assert.AreSame(_user1, actual);

                return;
            }

            if (creates)
            {
                // act
                var actual = userService.Get(username, userId);
                
                // assert
                Assert.IsNotNull(actual);
                Assert.IsInstanceOf<User>(actual);
                Assert.AreEqual(username, actual.Username);
                Assert.AreEqual(userId, actual.UserId);
                Assert.AreSame(_context.Users.First(u => u.Username == username), actual);

                return;
            }

            if (updates)
            {
                var actual = userService.Get(username, userId);
                
                Assert.IsNotNull(actual);
                Assert.IsInstanceOf<User>(actual);

                // assert
                if (username == null)
                {
                    Assert.AreEqual(userId, actual.Username);
                    return;
                }
                
                Assert.AreEqual(username, actual.Username);

                return;
            }
            
            Assert.Fail("Something went wrong");
        }
    }
}