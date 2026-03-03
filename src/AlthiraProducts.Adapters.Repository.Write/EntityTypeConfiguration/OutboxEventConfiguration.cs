using AlthiraProducts.Adapters.Repository.Write.EntitiesRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlthiraProducts.Adapters.Repository.Write.EntityTypeConfiguration;

public class OutboxEventConfiguration : IEntityTypeConfiguration<OutboxEventWriteModel>
{
    public void Configure(EntityTypeBuilder<OutboxEventWriteModel> builder)
    {
        builder.ToTable("OutboxEvents");
        builder.HasKey(x => x.Id);


        builder.Property(x => x.EventName)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.Version)
            .IsRequired();

        builder.Property(x => x.Payload)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

       
        builder.Property(x => x.RetryCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(x => x.Status)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.NextRetryAt);
        builder.Property(x => x.ProcessedAt);
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.Error)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(x => new { x.Status, x.NextRetryAt })
            .HasDatabaseName("IX_OutboxEvents_Status_NextRetry");
    }
}

