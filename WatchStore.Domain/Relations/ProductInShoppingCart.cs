using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain
{
    public class ProductInShoppingCart : BaseEntity
    {
        public Guid ProductId { get; set; }
        public virtual Product CurrnetProduct { get; set; }

        public Guid ShoppingCartId { get; set; }
        public virtual ShoppingCart UserCart { get; set; }

        public int Quantity { get; set; }
    }
}
