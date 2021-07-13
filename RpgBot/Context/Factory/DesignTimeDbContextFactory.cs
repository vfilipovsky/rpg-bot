using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RpgBot.Context.Factory
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BotContext>
    {
        public BotContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
 
            var builder = new DbContextOptionsBuilder<BotContext>();
            var connectionString = configuration.GetConnectionString("SQLiteConnection");
 
            builder.UseSqlite(connectionString);
 
            return new BotContext(builder.Options);
        }
    }
}