using AiAgentManager.DataBase.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiAgentManager.DataBase.Sqlite.Configurations
{
    public class ChatHistoryConfiguration : IEntityTypeConfiguration<ChatHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<ChatHistoryEntity> builder)
        {
            builder.ToTable("chathistories");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.ChatId)
                .IsRequired();
            builder.Property(a => a.SenderMessage)
                .IsRequired();
            builder.Property(a => a.Message)
                .IsRequired();
            builder.Property(a => a.DateSend)
                .IsRequired();
            builder.HasOne(a => a.SavedAgents)
                .WithMany(a => a.ChatHistory)
                .HasForeignKey(a => a.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
