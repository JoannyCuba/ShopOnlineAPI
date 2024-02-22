using ShopOnlineCore.Entity;

namespace ShopOnlineAPI.Models
{
    public class SaleModel : BaseModel
    {
        public DateTime SaleDate { get; set; }
        public int QuantitySold { get; set; }

        public string ClientModelId { get; set; }
        public ClientModel Client { get; set; }

        public string ProductModelId { get; set; }
        public ProductModel Product { get; set; }
    }
}
