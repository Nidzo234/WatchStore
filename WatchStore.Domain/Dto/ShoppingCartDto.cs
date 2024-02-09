using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchStore.Domain.Dto
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCartDto> Products { get; set; }

        public double TotalPrice { get; set; }
    }
}
