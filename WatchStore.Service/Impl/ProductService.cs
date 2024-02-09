using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Repository;

namespace WatchStore.Service
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;

        public ProductService(
            IRepository<Product> repository, 
            IUserRepository userRepository, 
            IRepository<ProductInShoppingCart> productInShoppingCartRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
        }

        public bool addToShoppingCart(Guid Id, AddToShoppingCardDto dto)
        {
            var user = this._userRepository.GetById(Id);
            if (user != null)
            {
                var userShoppingCard = user.UserCart;
                if (dto.SelectedProductId != Guid.Empty && userShoppingCard != null)
                {
                    var product = this.getProductDetails(dto.SelectedProductId);
                    if (product != null)
                    {
                        ProductInShoppingCart itemToAdd = new ProductInShoppingCart
                        {
                            Id = Guid.NewGuid(),
                            CurrnetProduct = product,
                            ProductId = product.Id,
                            UserCart = userShoppingCard,
                            ShoppingCartId = userShoppingCard.Id,
                            Quantity = dto.Quantity
                        };

                        var existing = userShoppingCard.ProductInShoppingCarts.Where(
                            z => z.ShoppingCartId == userShoppingCard.Id 
                            && z.ProductId == itemToAdd.ProductId).FirstOrDefault();

                        if (existing != null)
                        {
                            existing.Quantity += itemToAdd.Quantity;
                            this._productInShoppingCartRepository.Update(existing);

                        }
                        else
                        {
                            this._productInShoppingCartRepository.Insert(itemToAdd);
                        }
                        return true;
                    }
                    return false;
                }
            }
           return false;
        }

        public bool CreateProduct(Product product)
        {
            if(product != null)
            {
                _repository.Insert(product);
                return true;
            }
            return false;
           
        }

        public bool DeleteProduct(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return false;
            }
            Product p = _repository.Get(Id);
            if (p != null)
            {
                _repository.Delete(p);
                return true;
            }
            return false;
        }

        public Product getProductDetails(Guid Id)
        {
            if(Id != Guid.Empty)
            {
                return _repository.Get(Id);
            }
            return null;
            
        }

        public List<Product> GetProducts()
        {
            return _repository.GetAll().ToList();  
        }

        public bool updateProduct(Guid Id, ProductDto dto)
        {
            if (Id != Guid.Empty && dto !=null)
            {
                if (_repository.Get(Id) != null)
                {
                    Product p = new Product();
                    p.Id = Id;
                    p.ProductName = dto.ProductName;
                    p.ProductPrice = dto.ProductPrice;
                    p.ProductDescription = dto.ProductDescription;
                    p.ProductImage = dto.ProductImage;
                    p.ProductRating = dto.ProductRating;
                    _repository.Update(p);
                    return true;
                }
                return false;
            }
            return false;
            
        }
    }
}
