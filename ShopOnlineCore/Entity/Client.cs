
using ShopOnlineCore.Utils;
using System.Text.Json;

namespace ShopOnlineCore.Entity
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        //public List<Product> Products { get; set; }

        public override void Validate()
        {
            List<string> errors = new List<string>();
            if (!Validation.IsEmail(Email))
                errors.Add("Invalid Email.");
            if (PhoneNumber!= null && !Validation.IsPhoneNumber(PhoneNumber))
                errors.Add("Invalid Phone Number Format.");
            if (errors.Count > 0)
                throw new ArgumentException(JsonSerializer.Serialize(errors));
        }
    }
}
