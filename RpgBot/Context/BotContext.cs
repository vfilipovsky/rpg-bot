using Microsoft.EntityFrameworkCore;
using RpgBot.Entity;

namespace RpgBot.Context
{
    public class BotContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CommandAlias> CommandAliases { get; set; }

        public BotContext(DbContextOptions<BotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserId).IsUnique();
            modelBuilder.Entity<CommandAlias>().HasIndex(c => c.Alias).IsUnique();
        }
    }
}