using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ShopOnlineAPI.Models;
using System.Reflection.Emit;

namespace ShopOnlineAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductModel>()
               .ToTable("Product");

            builder.Entity<ClientModel>()
              .ToTable("Client");

            builder.Entity<ClientProductModel>(clientProduct =>
            {
                clientProduct.HasKey(x => new { x.ClientId, x.ProductId });

                clientProduct.HasOne(x => x.Client)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.ClientId)
                    .IsRequired(true);

                clientProduct.HasOne(x => x.Product)
                    .WithMany(x => x.Clients)
                    .HasForeignKey(x => x.ProductId)
                    .IsRequired(true);
            });
            builder.Entity<ProductModel>()
                    .Property(p => p.Price)
                    .HasColumnType("decimal(18, 2)");
        }

        public DbSet<ProductModel> Product { get; set; }
        public DbSet<ClientModel> Client { get; set; }
        public DbSet<ClientProductModel> ClientProducts { get; set; }

    }
}
