using Microsoft.EntityFrameworkCore;
using RpgBot.Entity;

namespace RpgBot.Context
{
    public class BotContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Id).IsUnique();
            });
            
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasIndex(e => e.Id).IsUnique();
            });
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=./Data/Database/bot.db");
        }
    }
}