using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Entity
{
    public class Sale : BaseEntity
    {
        public DateTime SaleDate { get; set; }
        public int QuantitySold { get; set; }
        public string Client { get; set; }
        public string Product { get; set; }
        public override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}
