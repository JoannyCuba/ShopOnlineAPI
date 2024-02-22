using ShopOnlineCore.Entity;
using System.ComponentModel.DataAnnotations;

namespace ShopOnlineAPI.Infrastructure.Dtos
{
    public class ClientDto
    {
        public string? id { get; set; }
        [Required(ErrorMessage = "The field name is required")]
        [StringLength(50, ErrorMessage = "The field name cannot exceed 50 characters")]
        public string name { get; set; } 
        [Required(ErrorMessage = "The field email is required")]
        [StringLength(50, ErrorMessage = "The field email cannot exceed 50 characters")]
        public string email { get; set; }
        public string? password { get; set; }
        public string? phoneNumber { get; set; }
        //public List<ProductDto>? products { get; set; }
    }

    public class UserInfo
    {
        public string? id { get; set; }
        [Required(ErrorMessage = "The field name is required")]
        [StringLength(50, ErrorMessage = "The field name cannot exceed 50 characters")]
        public string name { get; set; }
        [Required(ErrorMessage = "The field email is required")]
        [StringLength(50, ErrorMessage = "The field email cannot exceed 50 characters")]
        public string email { get; set; }
        public string? password { get; set; }
    }
}
