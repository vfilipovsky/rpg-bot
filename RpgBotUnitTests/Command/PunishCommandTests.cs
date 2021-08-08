﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Level.Abstraction;
using RpgBot.Service;
using RpgBot.Service.Abstraction;

namespace RpgBotUnitTests.Command
{
    public class PunishCommandTests
    {
        private Mock<IExperienceService> _mockExperienceService;
        private Mock<IRate> _mockRate;
        private Mock<ICommandArgsResolver> _mockCommandArgsResolver;
        private Mock<ILevelSystem> _mockLevelSystem;
        private Mock<IUserService> _mockUserService;

        private const int PunishStaminaCost = 25;

        [SetUp]
        public void SetUp()
        {
            _mockLevelSystem = new Mock<ILevelSystem>();
            _mockUserService = new Mock<IUserService>();
            _mockCommandArgsResolver = new Mock<ICommandArgsResolver>();
            _mockExperienceService = new Mock<IExperienceService>();
            _mockRate = new Mock<IRate>();

            _mockCommandArgsResolver
                .Setup(c => c.GetArgs("/punish @username", 1))
                .Returns(new List<string>()
                {
                    "/punish",
                    "@username"
                });

            _mockRate
                .Setup(r => r.PunishStaminaCost)
                .Returns(PunishStaminaCost);
        }

        [Test]
        public void RunPunishCommandWillThrowNotFoundExceptionIfUserNotFound()
        {
            // arrange
            var user = new User() { Username = "username2", StaminaPoints = 25 };

            _mockUserService
                .Setup(u => u.GetByUsername("username"))
                .Returns((User)null);

            var command =
                new PunishCommand(
                    new ExperienceService(
                        _mockLevelSystem.Object,
                        _mockUserService.Object,
                        _mockRate.Object),
                    _mockRate.Object, _mockCommandArgsResolver.Object);

            // assert
            Assert.Throws<NotFoundException>((() => command.Run($"/punish @username", user)));
        }

        [Test]
        public void RunPunishCommandWillSubtractsReputationFromUserIfFound()
        {
            // arrange
            var user = new User() { Username = "username2", StaminaPoints = 25 };

            _mockExperienceService
                .Setup(u => u.Punish("username", user))
                .Returns(new User() { Username = "username", Reputation = 1 });

            var command = new PunishCommand(_mockExperienceService.Object, _mockRate.Object,
                _mockCommandArgsResolver.Object);

            // act
            var actual = command.Run($"/punish @username", user);

            // asserts
            Assert.AreEqual("username got punished. 1 reputation in total.", actual);
        }

        [Test]
        public void RunPunishCommandCannotSucceedWhenNotEnoughStamina()
        {
            // arrange
            var command = new PunishCommand(_mockExperienceService.Object, _mockRate.Object,
                _mockCommandArgsResolver.Object);
            var user = new User() { Username = "username2", StaminaPoints = 20 };

            // act
            var actual = command.Run($"/punish @username", user);

            // asserts
            Assert.AreEqual($"Not enough stamina, need {PunishStaminaCost} (20).", actual);
        }

        [Test]
        public void PublicFieldsAreCorrect()
        {
            var command = new PunishCommand(
                new Mock<IExperienceService>().Object,
                new Mock<IRate>().Object,
                new Mock<ICommandArgsResolver>().Object);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("/punish", command.Name);
                Assert.AreEqual("Punish @user and subtracts reputation from him", command.Description);
                Assert.AreEqual(1, command.ArgsCount);
                Assert.AreEqual(3, command.RequiredLevel);
            });
        }
    }
}