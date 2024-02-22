using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Repositories;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;

namespace ShopOnlineAPI.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        public IRepository<Client> Client { get; set; }
        public IRepository<Product> Product { get; set; }
        public IRepository<Sale> Sale { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Client = new ClientRepository(context);
            Product = new ProductRepository(context);
            Sale = new SaleRepository(context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
