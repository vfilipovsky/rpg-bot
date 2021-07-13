using Microsoft.Extensions.DependencyInjection;
using RpgBot.Context;
using RpgBot.EntryPoint;
using RpgBot.Service;
using RpgBot.Service.Abstraction;

namespace RpgBot
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddSingleton<IUserService, UserService>();
            serviceCollection.AddSingleton<IGroupService, GroupService>();

            serviceCollection.AddDbContext<BotContext>();
            
            serviceCollection.AddTransient<BotContext>();
            serviceCollection.AddTransient<IEntryPoint, RpgBot.EntryPoint.Telegram>();

            return serviceCollection;
        }
    }
}