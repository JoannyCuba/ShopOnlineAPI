using System.ComponentModel.DataAnnotations;

namespace ShopOnlineAPI
{
    public class ProductDto
    {
        public string? id { get; set; }
        [Required(ErrorMessage = "The field name is required")]
        [StringLength(50, ErrorMessage = "The field name cannot exceed 50 characters")]
        public string name { get; set; }
        [StringLength(500, ErrorMessage = "The field description cannot exceed 500 characters")]
        public string? description { get; set; }
        public decimal? price { get; set; }
        public int? inStock { get; set; }
    }
}