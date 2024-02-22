using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? InStock { get; set; }

        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
