using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlthiraProducts.Adapters.Repository.Write.EntityTypeConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<ProductWriteModel>
{
    public void Configure(EntityTypeBuilder<ProductWriteModel> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
               .IsRequired()
               .ValueGeneratedNever();

        builder.Property(p => p.Sku)
               .IsRequired();

        builder.HasIndex(p => p.Sku)
               .IsUnique();

        builder.Property(p => p.Version)
               .IsRequired()
               .IsConcurrencyToken();

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.Price)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(p => p.Status)
               .IsRequired();

        builder.Property(p => p.Description)
                .IsRequired(false)
                .HasColumnType("nvarchar(max)");

        //Many Products → One Category
        builder.HasOne(p => p.Category)
               .WithMany()
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        //One Product → Many Images
        builder.HasMany(p => p.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
