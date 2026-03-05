using AlthiraProducts.Products.Application.Models.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlthiraProducts.Adapters.Repository.Write.EntityTypeConfiguration;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryWriteModel>
{
    public void Configure(EntityTypeBuilder<CategoryWriteModel> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .IsRequired()
               .ValueGeneratedNever();

        builder.Property(c => c.DeparmentName)
               .IsRequired()
               .HasMaxLength(150);
    }
}
