using AlthiraProducts.Products.Application.Models.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlthiraProducts.Adapters.Repository.Write.EntityTypeConfiguration;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageWriteModel>
{
    public void Configure(EntityTypeBuilder<ProductImageWriteModel> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(i => i.Name);

        builder.Property(i => i.Name)
            .IsRequired();

        builder.Property(i => i.Name)
            .IsRequired();

        builder.Property(i => i.ContentType)
            .IsRequired();

        builder.Property(i => i.Order)
            .IsRequired();

        builder.Property(i => i.ProductId)
               .IsRequired();

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
            .HasDatabaseName("IX_Images_Status_NextRetry");
    }
}