using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RpgBot.Bot;
using RpgBot.Bot.Telegram;
using RpgBot.Command;
using RpgBot.Command.Abstraction;
using RpgBot.Context;
using RpgBot.EntryPoint;
using RpgBot.Level;
using RpgBot.Level.Abstraction;
using RpgBot.Service;
using RpgBot.Service.Abstraction;
using Telegram.Bot.Types;

namespace RpgBot
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false);
            
            IConfiguration configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<IBot<Message, ChatId>, TelegramBot>();
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IExperienceService, ExperienceService>();
            serviceCollection.AddSingleton<IRate, Rate>();
            serviceCollection.AddSingleton<ILevelSystem, LevelSystem>();
            serviceCollection.AddSingleton<ICommandAliasService, CommandAliasService>();
            serviceCollection.AddSingleton<ICommandArgsResolver, CommandArgsResolver>();
            
            // commands
            serviceCollection.AddSingleton<CreateCommandAliasCommand>();
            serviceCollection.AddSingleton<DeleteCommandAliasCommand>();
            serviceCollection.AddSingleton<ListAliasesCommand>();
            serviceCollection.AddSingleton<TopCommand>();
            serviceCollection.AddSingleton<AboutCommand>();
            serviceCollection.AddSingleton<PraiseCommand>();
            serviceCollection.AddSingleton<PunishCommand>();
            serviceCollection.AddSingleton<MeCommand>();

            serviceCollection.AddSingleton<ICommands, Commands>();
            serviceCollection.AddSingleton<TelegramCommands>();

            serviceCollection.AddDbContext<BotContext>(options => {
                options.UseSqlite(configuration.GetConnectionString("SQLiteConnection"));
            });
            
            serviceCollection.AddTransient<BotContext>();
            serviceCollection.AddTransient<IEntryPoint, TelegramBot>();

            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
            });
            
            return serviceCollection;
        }
    }
}