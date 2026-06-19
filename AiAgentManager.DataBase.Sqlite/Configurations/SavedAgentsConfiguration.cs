using AiAgentManager.DataBase.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiAgentManager.DataBase.Sqlite.Configurations
{
    public class SavedAgentsConfiguration : IEntityTypeConfiguration<SavedAgentsEntity>
    {
        public void Configure(EntityTypeBuilder<SavedAgentsEntity> builder)
        {
            builder.ToTable("savedagents");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .IsRequired();
            builder.Property(x => x.PathExe)
                .IsRequired();
            builder.HasIndex(x => x.Name);
            builder.HasMany(a => a.ChatHistory)
                .WithOne(a => a.SavedAgents)
                .HasForeignKey(a => a.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
