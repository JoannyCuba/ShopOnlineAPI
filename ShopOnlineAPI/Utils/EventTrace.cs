using ShopOnlineAPI.Data;
using ShopOnlineAPI.Models;
using System.Text.Json;

namespace ShopOnlineAPI.Utils
{
    public class EventTrace : IEventTrace
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApplicationDbContext _context;
        public EventTrace(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = applicationDbContext;
        }
        public void AddTrace(string @event, object data)
        {
            string host = _httpContextAccessor.HttpContext.Request.Host.Host;
            string userId = "1111";
            _context.Trace.Add(new Trace()
            {
                Action = @event,
                UserId = userId,
                Date = DateTime.Now,
                Host = host,
                Data = JsonSerializer.Serialize(data)
            });
            _context.SaveChanges();
        }

        public List<Trace> GetTrace(DateTime startDate, DateTime endDate, string? userId, string? action)
        {
            var result = _context.Trace.Where(x =>
                (x.Date >= startDate && x.Date <= endDate)
                && (string.IsNullOrEmpty(userId) || x.UserId == userId)
                && (string.IsNullOrEmpty(action) || x.Action == action)
            ).ToList();
            return result;
        }
    }

    public interface IEventTrace
    {
        void AddTrace(string @event, object data);
        List<Trace> GetTrace(DateTime startDate, DateTime endDate, string? userId, string? action);
    }
}
