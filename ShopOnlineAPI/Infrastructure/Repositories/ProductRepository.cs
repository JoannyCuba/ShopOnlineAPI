using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;

namespace ShopOnlineAPI.Infrastructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private DbSet<Models.ProductModel> dbSet;

        public ProductRepository(ApplicationDbContext context)
        {
            dbSet = context.Set<Models.ProductModel>();
        }
        public void Add(Product entity)
        {
            var model = AutoMapperProfile.Map<Product, ProductModel>(entity, true);
            model.CreatedAt = DateTime.Now;
            dbSet.Add(model);
        }

        public int Count(object search = null)
        {
            FilterDto filter = (FilterDto)search;
            return dbSet.
                    Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.Name.Contains(filter.search))
                            && x.DeletedAt == null).Count();
        }

        public List<Product> Find(object? search = null, int page = 1, int itemPerPage = 25)
        {
            FilterDto filter = search == null ? new FilterDto()
            {
                search = null,
            } : (FilterDto)search;
            var result = dbSet
                .Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.Name.Contains(filter.search))
                            && x.DeletedAt == null)
                .Skip((page - 1) * itemPerPage)
                .Take(itemPerPage)
                .OrderBy(x => x.Name)
                .Select(x =>
                    AutoMapperProfile.Map<ProductModel, Product>(x, true)
                ).ToList();
            return result;
        }

        public Product FindOne(string Id)
        {
            var result = dbSet.FirstOrDefault(x => x.Id == Id);
            return AutoMapperProfile.Map<ProductModel, Product>(result, true);
        }

        public void Remove(Product entity)
        {
            var result = dbSet.FirstOrDefault(x => x.Id == entity.Id);
            if (result != null)
            {
                result.UpdatedAt = DateTime.Now;
                result.DeletedAt = DateTime.Now;
                dbSet.Update(result);
            }
        }

        public void Update(Product entity)
        {
            var result = dbSet.FirstOrDefault(x => x.Id == entity.Id);
            if (result != null)
            {
                result.Name = entity.Name;
                result.Description = entity.Description;
                result.Price = entity.Price;
                result.InStock = entity.InStock;
                result.UpdatedAt = DateTime.Now;
                dbSet.Update(result);
            }
        }
    }
}
