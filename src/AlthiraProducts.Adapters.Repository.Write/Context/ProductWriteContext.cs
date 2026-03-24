using AlthiraProducts.Adapters.Repository.Write.EntityTypeConfiguration;
using AlthiraProducts.BoundedContext.Products.Application.Models.Persistence.Write;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AlthiraProducts.Adapters.Repository.Write.Context;

public class ProductWriteContext : DbContext
{
    public DbSet<ProductWriteModel> Products { get; set; }
    public DbSet<CategoryWriteModel> Categories{ get; set; }
    public DbSet<ProductImageWriteModel> Images{ get; set; }
    public DbSet<OutboxEventWriteModel> OutboxEvents { get; set; }

    public ProductWriteContext(DbContextOptions<ProductWriteContext> options) : base(options) { }
    public ProductWriteContext()
    {
    }//Este ctor para cuando es code first

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxEventConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Esto lo usaremos si queremos coger la cadena de conwxion desde este proyecto
        if (!optionsBuilder.IsConfigured)
        {
            string appsettingsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..",
                "AlthiraProducts.Main",
                "appsettings.json"
            );

            string connectionString = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(appsettingsPath)!)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<ProductWriteContext>()
                .AddEnvironmentVariables()
                .Build()
                .GetSection("Config")["DatabaseWrite:ConnectionString"]!;

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}