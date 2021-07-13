using Microsoft.Extensions.DependencyInjection;
using RpgBot.EntryPoint;

namespace RpgBot
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var services = Startup.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<IEntryPoint>()?.Run(args);
        }
    }
}