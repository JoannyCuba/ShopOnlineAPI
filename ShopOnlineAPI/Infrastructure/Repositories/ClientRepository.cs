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

        /// <summary>
        /// Counts the non-deleted client records that optionally satisfy a search condition on name or email.
        /// </summary>
        /// <param name="search">
        /// An optional filter object (expected to be of type FilterDto) containing a search string. If the search string is null or empty, the count includes all non-deleted clients.
        /// </param>
        /// <returns>
        /// The number of client records matching the specified criteria.
        /// </returns>
        public int Count(object search = null)
        {
            FilterDto filter = (FilterDto)search;
            return dbSet.Include(x => x.Products)
                .Where(x => (string.IsNullOrEmpty(filter.search)
                            || x.Name.Contains(filter.search)
                            || x.Email.Contains(filter.search))
                            && x.DeletedAt == null).Count();
        }

        /// <summary>
        /// Retrieves a paginated list of client entities that match an optional search criteria.
        /// </summary>
        /// <param name="search">
        /// An optional filter used to match clients by name or email. When provided, it should be a FilterDto with the search term specified; if null, no filtering is applied.
        /// </param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="itemPerPage">The number of items per page. Defaults to 25.</param>
        /// <returns>
        /// A list of clients that are not marked as deleted and whose name or email contains the search term (if provided).
        /// </returns>
        public List<Client> Find(object? search = null, int page = 1, int itemPerPage = 25)
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
