using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStore.Service
{
    public interface IProductService
    {
        List<Product> GetProducts();
        bool CreateProduct(Product product);
        bool DeleteProduct(Guid Id);
        Product getProductDetails(Guid Id);
        bool updateProduct(Guid Id, ProductDto dto);
        bool addToShoppingCart(Guid Id, AddToShoppingCardDto dto);
        bool updateQuantity(Guid productId, Guid userId,  int quantity);
    }
}
