using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain.Dto;

namespace WatchStore.Domain
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public List<ProductInShoppingCartDto> products { get; set; }
        public double totalPrice { get; set; }
    }
}
