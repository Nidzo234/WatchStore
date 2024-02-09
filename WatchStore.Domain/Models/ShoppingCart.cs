using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain
{
    public class ShoppingCart : BaseEntity
    {
        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }

    }
}
