using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain
{
    public class Order :BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
