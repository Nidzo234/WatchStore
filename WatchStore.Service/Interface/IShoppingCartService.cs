using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Domain.Dto;

namespace WatchStore.Service.Interface
{
    public interface IShoppingCartService
    {
        public ShoppingCartDto getShoppingCartInfo(User u);
        bool order(User u);
        bool deleteProductFromSoppingCart(Guid userId, Guid productId);
    }
}
