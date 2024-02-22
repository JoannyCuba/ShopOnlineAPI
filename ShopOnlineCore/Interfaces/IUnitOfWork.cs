using ShopOnlineCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Interfaces
{
    public interface IUnitOfWork
    {
        public IRepository<Client> Client { get; set; }
        public IRepository<Product> Product { get; set; }
        public IRepository<Sale> Sale { get; set; }
        public void Save();
        public Task SaveAsync();
        public void BeginTransaction();
        public void CommitTransaction();
        public void RollbackTransaction();
    }
}
