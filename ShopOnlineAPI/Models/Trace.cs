namespace ShopOnlineAPI.Models
{
    public class Trace
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string Action { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
        public string Host { get; set; }
    }
}
