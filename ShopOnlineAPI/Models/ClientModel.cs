using ShopOnlineCore.Entity;

namespace ShopOnlineAPI.Models
{
    public class ClientModel : BaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<ClientProductModel>? Products { get; set; }
        public ICollection<SaleModel>? Sales { get; set; } 
    }
}
