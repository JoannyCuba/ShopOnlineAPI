namespace ShopOnlineAPI.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? InStock { get; set;}
        public ICollection<ClientProductModel>? Clients { get; set; }

    }
}
