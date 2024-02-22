using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Utils
{
    public class Constants
    {
        public static class EventCore
        {
            // Clients
            public const string ReadClient = "read_client";
            public const string AddClient = "add_client";
            public const string UpdateClient = "update_client";
            public const string DeleteClient = "delete_client";
           
            // Products
            public const string ReadProduct = "read_product";
            public const string AddProduct = "add_product";
            public const string UpdateProduct = "update_product";
            public const string DeleteProduct = "delete_product";

            //Sales
            public const string ReadSale = "read_sale";
            public const string AddSale = "add_sale";

        }
    }
}
