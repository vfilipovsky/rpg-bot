using RpgBot.Entity;
using RpgBot.Exception;
using RpgBot.Level.Abstraction;
using RpgBot.Service.Abstraction;
using RpgBot.Type;

namespace RpgBot.Service
{
    public class ExperienceService : IExperienceService
    {
        private readonly ILevelSystem _levelSystem;
        private readonly IUserService _userService;
        private readonly IRate _rate;

        public ExperienceService(ILevelSystem levelSystem, IUserService userService, IRate rate)
        {
            _levelSystem = levelSystem;
            _userService = userService;
            _rate = rate;
        }

        public User Praise(string username, User user)
        {
            var userToPraise = _userService.GetByUsername(username);

            if (user.ManaPoints < _rate.PraiseManaCost)
                throw new NotEnoughManaException($"Not enough mana, need {_rate.PraiseManaCost} ({user.ManaPoints}).");

            if (null == userToPraise)
                throw new NotFoundException($"User '{username}' not found");

            user.ManaPoints -= _rate.PraiseManaCost;
            userToPraise.Reputation += _rate.ReputationPerPraise;

            _userService.Update(user);

            return _userService.Update(userToPraise);
        }

        public User Punish(string username, User user)
        {
            var userToPunish = _userService.GetByUsername(username);

            if (user.StaminaPoints < _rate.PunishStaminaCost)
                throw new NotEnoughStaminaException(
                    $"Not enough stamina, need {_rate.PunishStaminaCost} ({user.StaminaPoints}).");

            if (null == userToPunish)
                throw new NotFoundException($"User '{username}' not found");

            user.StaminaPoints -= _rate.PunishStaminaCost;
            userToPunish.Reputation += _rate.ReputationPerPunish;

            _userService.Update(user);

            return _userService.Update(userToPunish);
        }

        public User Regenerate(User user)
        {
            var hpAfterRegen = user.HealthPoints + _rate.HealthRegen;
            var manaAfterRegen = user.ManaPoints + _rate.ManaRegen;
            var staminaAfterRegen = user.StaminaPoints + _rate.StaminaRegen;

            if (hpAfterRegen <= user.MaxHealthPoints) user.HealthPoints = hpAfterRegen;
            if (manaAfterRegen <= user.MaxManaPoints) user.ManaPoints = manaAfterRegen;
            if (staminaAfterRegen <= user.MaxStaminaPoints) user.StaminaPoints = staminaAfterRegen;

            return user;
        }

        public User AddExpForMessage(User user, MessageType type)
        {
            _levelSystem.AddExp(user, type);
            user.MessagesCount += 1;

            if (user.MessagesCount % _rate.RegeneratePerMessages == 0)
                user = Regenerate(user);

            return _userService.Update(user);
        }
    }
}