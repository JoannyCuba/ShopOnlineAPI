namespace ShopOnlineAPI.Models
{
    public class ClientProductModel
    {
        public string ClientId { get; set; }
        public string ProductId { get; set; }
        public ClientModel Client { get; set; }
        public ProductModel Product { get; set; }
    }
}
