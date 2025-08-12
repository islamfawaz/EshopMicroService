using BuildingBlocks.Messaging.Outbox;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)"); 

            builder.Property(x => x.OccurredOnUtc)
                .IsRequired();

            builder.Property(x => x.ProcessedOnUtc)
                .IsRequired(false);

            builder.Property(x => x.Error)
                .HasMaxLength(2000)  
                .IsRequired(false);

            builder.HasIndex(x => x.ProcessedOnUtc);
        }
    }
}
