using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Domain.Entities;

namespace TodoApp.Infrastructure.Database.Configurations
{
    internal sealed class QuestConfiguration : IEntityTypeConfiguration<Quest>
    {
        public void Configure(EntityTypeBuilder<Quest> builder)
        {
            builder.HasKey(q => q.Id);
            builder.Property(q => q.Id)
                .UseMySqlIdentityColumn();

            builder.Property(q=>q.Title)
                .HasMaxLength(150);

            builder.Property(q => q.Description)
                .HasMaxLength(3000);

            builder.Property(q => q.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(q => q.Created);
            builder.Property(q => q.Modified);

            builder.HasIndex(q => new { q.Title, q.Status });
            builder.HasIndex(q => q.Status);
        }
    }
}
