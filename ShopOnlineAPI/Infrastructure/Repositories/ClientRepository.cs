using Microsoft.EntityFrameworkCore;
using ShopOnlineAPI.Data;
using ShopOnlineAPI.Infrastructure.Dtos;
using ShopOnlineAPI.Models;
using ShopOnlineAPI.Utils;
using ShopOnlineCore.Entity;
using ShopOnlineCore.Interfaces;

namespace ShopOnlineAPI.Infrastructure.Repositories
{
    public class ClientRepository : IRepository<Client>
    {
        private DbSet<Models.ClientModel> dbSet;

        public ClientRepository(ApplicationDbContext context)
        {
            dbSet = context.Set<Models.ClientModel>();
        }

        public void Add(Client entity)
        {
            var model = AutoMapperProfile.Map<Client, ClientModel>(entity, true);
            model.CreatedAt = DateTime.Now;
            dbSet.Add(model);
        }

        public int Count(object search = null)
        {
            FilterDto filter = (FilterDto)search;
            return dbSet.Include(x => x.Products)
                .Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.Name.Contains(filter.search)
                            || x.Email.Contains(filter.search))
                            && x.DeletedAt == null).Count();
        }

        public List<Client> Find(object search = null, int page = 1, int itemPerPage = 25)
        {
            FilterDto filter = search == null ? new FilterDto()
            {
                search = null,
            } : (FilterDto)search;
            var result = dbSet
                .Include(x => x.Products)
                .Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.Name.Contains(filter.search)
                            || x.Email.Contains(filter.search))
                            && x.DeletedAt == null)
                .Skip((page - 1) * itemPerPage)
                .Take(itemPerPage)
                .OrderBy(x => x.Name)
                .Select(x =>
                    AutoMapperProfile.Map<ClientModel, Client>(x, true)
                ).ToList();
            return result;
        }

        public Client FindOne(string Id)
        {
            var result = dbSet.Include(x => x.Products).FirstOrDefault(x => x.Id == Id);
            return AutoMapperProfile.Map<ClientModel, Client>(result, true);
        }

        public void Remove(Client entity)
        {
            var result = dbSet.FirstOrDefault(x => x.Id == entity.Id);
            if (result != null)
            {
                result.UpdatedAt = DateTime.Now;
                result.DeletedAt = DateTime.Now;
                dbSet.Update(result);
            }
        }

        public void Update(Client entity)
        {
            var result = dbSet.FirstOrDefault(x => x.Id == entity.Id);
            if (result != null)
            {
                result.Name = entity.Name;
                result.Email = entity.Email;
                result.PhoneNumber = entity.PhoneNumber;
                result.UpdatedAt = DateTime.Now;
                dbSet.Update(result);
            }
        }
    }
}
