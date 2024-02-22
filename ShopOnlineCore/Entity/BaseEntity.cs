using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnlineCore.Entity
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public static string CreateUUID()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Validate the entity
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public abstract void Validate();
    }
}
