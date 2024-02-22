using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;

namespace ShopOnlineAPI.Infrastructure.Repositories
{
    public class SaleRepository : IRepository<Sale>
    {
        private DbSet<Models.SaleModel> dbSet;
        public SaleRepository(ApplicationDbContext context)
        {
            dbSet = context.Set<Models.SaleModel>();
        }
        public void Add(Sale entity)
        {
            var model = AutoMapperProfile.Map<Sale, SaleModel>(entity, true);
            model.SaleDate = DateTime.Now;
            model.CreatedAt = DateTime.Now;
            dbSet.Add(model);
        }

        public int Count(object search = null)
        {
            throw new NotImplementedException();
        }

        public List<Sale> Find(object search = null, int page = 1, int itemPerPage = 25)
        {
            FilterDto filter = search == null ? new FilterDto()
            {
                search = null,
            } : (FilterDto)search;
            var result = dbSet
                .Include(x => x.Product)
                .Include(x => x.Client)
                .Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.ClientModelId.Equals(filter.search)
                            || x.ProductModelId.Equals(filter.search))
                            && x.DeletedAt == null)
                .Skip((page - 1) * itemPerPage)
                .Take(itemPerPage)
                .OrderBy(x => x.SaleDate)
                .Select(x =>
                    AutoMapperProfile.Map<SaleModel, Sale>(x, true)
                ).ToList();
            return result;
        }

        public Sale FindOne(string Id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Sale entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Sale entity)
        {
            throw new NotImplementedException();
        }
    }
}
