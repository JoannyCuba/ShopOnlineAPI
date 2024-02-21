using Microsoft.EntityFrameworkCore;

namespace ShopOnlineAPI.Data
{
    public class SqlDbContext : ApplicationDbContext
    {
        public SqlDbContext(IConfiguration configuration) : base(configuration) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration["Database:ConnectionString"]);
        }
    }
}