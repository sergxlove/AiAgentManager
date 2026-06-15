using AiAgentManager.DataBase.Sqlite.Configurations;
using AiAgentManager.DataBase.Sqlite.Models;
using Microsoft.EntityFrameworkCore;

namespace AiAgentManager.DataBase.Sqlite
{
    public class AiAgentManagerDbContext : DbContext
    {
        public AiAgentManagerDbContext(DbContextOptions<AiAgentManagerDbContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<SavedAgentsEntity> SavedAgents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SavedAgentsConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
