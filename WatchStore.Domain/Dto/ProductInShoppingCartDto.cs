using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain.Dto
{
    public class ProductInShoppingCartDto
    {
        public Guid id { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public double ProductPrice { get; set; }
        public int quantity { get; set; }
    }
}
