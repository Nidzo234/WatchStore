using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Repository;
using WatchStore.Service;

namespace WatchStoreTest
{
    [TestFixture]
    public class ProductServiceTests
    {
        private ProductService _productService;
        private Mock<IRepository<Product>> _productRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRepository<ProductInShoppingCart>> _productInShoppingCartRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _productInShoppingCartRepositoryMock = new Mock<IRepository<ProductInShoppingCart>>();
            _productService = new ProductService(
                _productRepositoryMock.Object, 
                _userRepositoryMock.Object, 
                _productInShoppingCartRepositoryMock.Object);
        }


        [Test]
        public void CreateProduct_ShouldCallInsertMethodOfProductRepository()
        {
            // Arrange
            var product = new Product();
            product.ProductName = "TestName";
            product.ProductImage = "TestImage";
            product.ProductDescription = "TestDescription";
            product.ProductPrice = 210;
            product.ProductRating =4.4;

            // Act
            var result = _productService.CreateProduct(product);

            // Assert
            Assert.IsTrue(result, "Expected product to be created successfully");
            _productRepositoryMock.Verify(repo => repo.Insert(product), Times.Once);
        }

        [Test]
        public void CreateProduct_ValidProduct_ShouldInsertProduct()
        {
            // Arrange
            var product = new Product();

            // Act
            var result = _productService.CreateProduct(product);

            // Assert
            Assert.IsTrue(result, "Expected product to be created successfully");
            _productRepositoryMock.Verify(repo => repo.Insert(product), Times.Once);
        }


        [Test]
        public void CreateProduct_NullProduct_ShouldNotInsertProduct()
        {
            // Arrange
            Product product = null;

            // Act
            var result = _productService.CreateProduct(product);

            // Assert
            Assert.IsFalse(result, "Expected product not to be created");
            _productRepositoryMock.Verify(repo => repo.Insert(It.IsAny<Product>()), Times.Never);
        }


        [Test]
        public void DeleteProduct_EmptyId_ShouldNotDeleteProduct()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            var result = _productService.DeleteProduct(emptyId);

            // Assert
            Assert.IsFalse(result, "Expected product not to be deleted");
            _productRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Product>()), Times.Never);
        }



        [Test]
        public void DeleteProduct_ProductDoesNotExists_ShouldNotCallDeleteMethodOfProductRepository()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.Get(productId)).Returns((Product)null);

            // Act
            var result = _productService.DeleteProduct(productId);

            // Assert
            Assert.IsFalse(result);
            _productRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Product>()), Times.Never);
        }



        [Test]
        public void DeleteProduct_ProductExists_ShouldCallDeleteMethodOfProductRepository()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productToDelete = new Product { Id = productId };
            _productRepositoryMock.Setup(repo => repo.Get(productId)).Returns(productToDelete);

            // Act
            var result = _productService.DeleteProduct(productId);

            // Assert
            Assert.IsTrue(result, "Expected DeleteProduct to return true");
            _productRepositoryMock.Verify(repo => repo.Delete(productToDelete), Times.Once);
        }





        [Test]
        public void GetProducts_ShouldReturnListOfProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), ProductName = "Product 1" },
            new Product { Id = Guid.NewGuid(), ProductName = "Product 2" },
            new Product { Id = Guid.NewGuid(), ProductName = "Product 3" }
        };
            _productRepositoryMock.Setup(repo => repo.GetAll()).Returns(products);

            // Act
            var result = _productService.GetProducts();

            // Assert
            Assert.IsNotNull(result, "Expected list of products not to be null");
            Assert.AreEqual(products.Count, result.Count, "Expected number of products to match");
            Assert.IsTrue(products.SequenceEqual(result), "Expected products to be equal");
            _productRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }



        [Test]
        public void UpdateProduct_ValidIdAndDto_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var dto = new ProductDto
            {
                ProductName = "Updated Name",
                ProductPrice = 100,
                ProductDescription = "Updated Description",
                ProductImage = "Updated Image",
                ProductRating = 4.5
            };
            var existingProduct = new Product { Id = productId };

            _productRepositoryMock.Setup(repo => repo.Get(productId)).Returns(existingProduct);

            // Act
            var result = _productService.updateProduct(productId, dto);

            // Assert
            Assert.IsTrue(result, "Expected product to be updated");
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        }



        [Test]
        public void UpdateProduct_EmptyId_ShouldNotUpdateProduct()
        {
            // Arrange
            var emptyId = Guid.Empty;
            var dto = new ProductDto
            {
                ProductName = "Updated Name",
                ProductPrice = 100,
                ProductDescription = "Updated Description",
                ProductImage = "Updated Image",
                ProductRating = 4.5
            };

            // Act
            var result = _productService.updateProduct(emptyId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be updated");
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        }


        [Test]
        public void UpdateProduct_NullDto_ShouldNotUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            var result = _productService.updateProduct(productId, null);

            // Assert
            Assert.IsFalse(result, "Expected product not to be updated");
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        }



        [Test]
        public void UpdateProduct_ProductDoesNotExist_ShouldNotUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var dto = new ProductDto
            {
                ProductName = "Updated Name",
                ProductPrice = 100,
                ProductDescription = "Updated Description",
                ProductImage = "Updated Image",
                ProductRating = 4.5
            };

            _productRepositoryMock.Setup(repo => repo.Get(productId)).Returns((Product)null);

            // Act
            var result = _productService.updateProduct(productId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be updated");
            _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public void GetProductDetails_ValidProductId_ShouldReturnProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var expectedProduct = new Product { Id = productId };
            _productRepositoryMock.Setup(repo => repo.Get(productId)).Returns(expectedProduct);

            // Act
            var result = _productService.getProductDetails(productId);

            // Assert
            Assert.AreEqual(expectedProduct, result, "Expected product details to match");
            _productRepositoryMock.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
        }


        [Test]
        public void GetProductDetails_EmptyId_ShouldReturnNull()
        {
            // Arrange
            var emptyId = Guid.Empty;

            // Act
            var result = _productService.getProductDetails(emptyId);

            // Assert
            Assert.IsNull(result, "Expected null product details for empty ID");
            _productRepositoryMock.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Never);
        }


        [Test]
        public void GetProductDetails_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.Get(invalidId)).Returns((Product)null);

            // Act
            var result = _productService.getProductDetails(invalidId);

            // Assert
            Assert.IsNull(result, "Expected null product details for invalid ID");
            _productRepositoryMock.Verify(repo => repo.Get(It.IsAny<Guid>()), Times.Once);
        }


        [Test]
        public void GetProductDetails_ShouldCallGetMethodOfProductRepository()
        {
            // Arrange
            var productId = Guid.NewGuid();

            // Act
            _productService.getProductDetails(productId);

            // Assert
            _productRepositoryMock.Verify(repo => repo.Get(productId), Times.Once);
        }

        [Test]
        public void AddToShoppingCart_ValidData_ShouldUpdateProductToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var shoppingCartId = Guid.NewGuid();

            var userShoppingCart = new ShoppingCart { Id = shoppingCartId, OwnerId = userId };
            var dto = new AddToShoppingCardDto { SelectedProductId = productId, Quantity = 1 };
            var product = new Product { Id = productId };
            
            var pis = new ProductInShoppingCart
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                CurrnetProduct = product,
                UserCart = userShoppingCart,
                Quantity = 5,
                Id = Guid.NewGuid()
            };
            var productInShoppingCarts = new List<ProductInShoppingCart>();
            productInShoppingCarts.Add(pis);
            userShoppingCart.ProductInShoppingCarts = productInShoppingCarts;
            var user = new User { Id = userId, UserCart = userShoppingCart };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _productRepositoryMock.Setup(service => service.Get(productId)).Returns(product);
            _productInShoppingCartRepositoryMock.Setup(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()));

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsTrue(result, "Expected product to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Once);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Never);
        }


        [Test]
        public void AddToShoppingCart_ValidData_ShouldAddProductToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var shoppingCartId = Guid.NewGuid();
            var productToAddId = Guid.NewGuid();

            var userShoppingCart = new ShoppingCart { Id = shoppingCartId, OwnerId = userId };
            var dto = new AddToShoppingCardDto { SelectedProductId = productToAddId, Quantity = 1 };
            var product = new Product { Id = productId };
            var productToAdd = new Product { Id = productToAddId };

            var pis = new ProductInShoppingCart
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                CurrnetProduct = product,
                UserCart = userShoppingCart,
                Quantity = 5,
                Id = Guid.NewGuid()
            };
            var productInShoppingCarts = new List<ProductInShoppingCart>();
            productInShoppingCarts.Add(pis);
            userShoppingCart.ProductInShoppingCarts = productInShoppingCarts;
            var user = new User { Id = userId, UserCart = userShoppingCart };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _productRepositoryMock.Setup(service => service.Get(productToAddId)).Returns(productToAdd);
            _productInShoppingCartRepositoryMock.Setup(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()));

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsTrue(result, "Expected product to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Never);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Once);
        }




        [Test]
        public void AddToShoppingCart_ValidData_ProductNotExists_ShouldNotAddToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var shoppingCartId = Guid.NewGuid();
            var productToAddId = Guid.NewGuid();

            var userShoppingCart = new ShoppingCart { Id = shoppingCartId, OwnerId = userId };
            var dto = new AddToShoppingCardDto { SelectedProductId = productToAddId, Quantity = 1 };
            var product = new Product { Id = productId };

            var pis = new ProductInShoppingCart
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                CurrnetProduct = product,
                UserCart = userShoppingCart,
                Quantity = 5,
                Id = Guid.NewGuid()
            };
            var productInShoppingCarts = new List<ProductInShoppingCart>();
            productInShoppingCarts.Add(pis);
            userShoppingCart.ProductInShoppingCarts = productInShoppingCarts;
            var user = new User { Id = userId, UserCart = userShoppingCart };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _productRepositoryMock.Setup(service => service.Get(productToAddId)).Returns((Product)null);
            _productInShoppingCartRepositoryMock.Setup(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()));

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Never);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Never);
        }




        [Test]
        public void AddToShoppingCart_UserShoppingCartNotExists_ShouldNotAddToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var shoppingCartId = Guid.NewGuid();
            var productToAddId = Guid.NewGuid();

            var dto = new AddToShoppingCardDto { SelectedProductId = productToAddId, Quantity = 1 };
            var product = new Product { Id = productId };
            var user = new User { Id = userId, UserCart = null};

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _productInShoppingCartRepositoryMock.Setup(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()));

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Never);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Never);
        }



        [Test]
        public void AddToShoppingCart_ValidData_SelectedProductIdIsEmpty_ShouldNotAddToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var shoppingCartId = Guid.NewGuid();
            var productToAddId = Guid.NewGuid();

            var userShoppingCart = new ShoppingCart { Id = Guid.Empty, OwnerId = userId };
            var dto = new AddToShoppingCardDto { SelectedProductId = productToAddId, Quantity = 1 };
            var product = new Product { Id = productId };

            var pis = new ProductInShoppingCart
            {
                ProductId = productId,
                ShoppingCartId = shoppingCartId,
                CurrnetProduct = product,
                UserCart = userShoppingCart,
                Quantity = 5,
                Id = Guid.NewGuid()
            };
            var productInShoppingCarts = new List<ProductInShoppingCart>();
            productInShoppingCarts.Add(pis);
            userShoppingCart.ProductInShoppingCarts = productInShoppingCarts;
            var user = new User { Id = userId, UserCart = userShoppingCart };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns(user);
            _productInShoppingCartRepositoryMock.Setup(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()));

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Never);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Never);
        }





        [Test]
        public void AddToShoppingCart_UserNotExists_ShouldNotAddToShoppingCart()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var dto = new AddToShoppingCardDto { SelectedProductId = productId, Quantity = 1 };

            _userRepositoryMock.Setup(repo => repo.GetById(userId)).Returns((User)null);

            // Act
            var result = _productService.addToShoppingCart(userId, dto);

            // Assert
            Assert.IsFalse(result, "Expected product not to be added to the shopping cart");
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Update(It.IsAny<ProductInShoppingCart>()), Times.Never);
            _productInShoppingCartRepositoryMock.Verify(repo => repo.Insert(It.IsAny<ProductInShoppingCart>()), Times.Never);
        }


    }
}
