namespace ShopOnlineAPI.Models
{
    public class ProductModel : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? InStock { get; set;}
        public ICollection<ClientProductModel>? Clients { get; set; }
        public ICollection<SaleModel>? Sales { get; set; }

    }
}
