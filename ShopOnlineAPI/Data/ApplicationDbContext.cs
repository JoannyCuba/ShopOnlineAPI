using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Models;

namespace ShopOnlineAPI.Data
{
    /// <summary>
    /// Represents the database context for the application, providing a connection to the database
    /// and serving as a gateway for interacting with the underlying database using Entity Framework Core.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Gets the configuration instance that provides access to the application's configuration settings,
        /// allowing the <see cref="ApplicationDbContext"/> to access the necessary configuration information.
        /// </summary>
        protected readonly IConfiguration Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class
        /// with the specified configuration, allowing the application to access the database.
        /// </summary>
        /// <param name="configuration">The configuration used to set up the database connection.</param>

        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Called when creating the model for the database and allows configuring relationships,
        /// properties, and behaviors of entities within the Entity Framework Core context.
        /// </summary>
        /// <param name="builder">The model builder used to configure the database model.</param>
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

            builder.Entity<SaleModel>(sale =>
            {
                sale.ToTable("Sales"); // Asegúrate de que sea el mismo nombre de tabla que estás utilizando para SaleModel en tu base de datos
                sale.HasKey(x => x.Id);

                sale.HasOne(x => x.Client)
                    .WithMany(x => x.Sales)
                    .HasForeignKey(x => x.ClientModelId)
                    .IsRequired(true);

                sale.HasOne(x => x.Product)
                    .WithMany(x => x.Sales)
                    .HasForeignKey(x => x.ProductModelId)
                    .IsRequired(true);
            });
        }

        /// <summary>
        /// Represents Product table in the data base.
        /// </summary>
        public DbSet<ProductModel> Product { get; set; }

        /// <summary>
        /// Represents Client table in the data base.
        /// </summary>
        public DbSet<ClientModel> Client { get; set; }

        /// <summary>
        /// Represents the many to many relationship between Clients and Products tables in the data base.
        /// </summary>
        public DbSet<ClientProductModel> ClientProducts { get; set; }

        /// <summary>
        /// Represents Traces table in the data base.
        /// </summary>
        public DbSet<Trace> Trace { get; set; }

        /// <summary>
        /// Represents Sales table in the data base.
        /// </summary>
        public DbSet<SaleModel> Sale { get; set; }


    }
}
