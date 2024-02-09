using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStoreTest.IntegrationTests.ControllerTests
{
    public class AdminControllerTests : IDisposable
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        public AdminControllerTests()
        {
            _factory = new CustomWebApplicationFactory();
            _client= _factory.CreateClient();
        }


        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }


        [Fact]
        public async Task get_AllProducts()
        {
            var allProducts = new Product[]
            {
                new()
                {
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };

            _factory.ProductRepositoryMock.Setup(p => p.GetAll()).Returns(allProducts);
            
            var response = await _client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync());


            Assert.Collection((data as IEnumerable<Product>),
                r => {
                    Assert.Equal("Test", r.ProductName);
                 },
                r =>
                {
                    Assert.Equal("Test2", r.ProductName);
                });
        }

        [Fact]
        public async Task GetById_IfExists_ReturnProduct()
        {
            var allProducts = new Product[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };
               for(int i=0;i<allProducts.Length;i++)
            {
                _factory.ProductRepositoryMock.Setup(p => p.Get(allProducts[i].Id)).Returns(allProducts[i]);
            }
            

            var response = await _client.GetAsync("/api/Admin/getProduct?id=" + allProducts[0].Id);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());


            Assert.Equal(allProducts[0].Id, data!.Id);
            Assert.Equal("Test", data!.ProductName);
               
        }


        [Fact]
        public async Task GetById_IfMissing_Return404t()
        {
            var allProducts = new Product[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };

            for (int i = 0; i < allProducts.Length; i++)
            {
                _factory.ProductRepositoryMock.Setup(p => p.Get(allProducts[i].Id)).Returns(allProducts[i]);
            }

            var response = await _client.GetAsync("/api/Admin/getProduct?id=" + Guid.NewGuid());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }



        [Fact]
        public async Task DeleteById_IfExists_ReturnOk()
        {
            var allProducts = new Product[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };

            for (int i = 0; i < allProducts.Length; i++)
            {
                _factory.ProductRepositoryMock.Setup(p => p.Get(allProducts[i].Id)).Returns(allProducts[i]);
            }

            var respons1 = await _client.DeleteAsync("/api/Admin/deleteProduct?id=" + allProducts[0].Id);
            Assert.Equal(HttpStatusCode.OK, respons1.StatusCode);


        }



        [Fact]
        public async Task DeleteById_IfMissing_Return404()
        {
            var allProducts = new Product[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };

            for (int i = 0; i < allProducts.Length; i++)
            {
                _factory.ProductRepositoryMock.Setup(p => p.Get(allProducts[i].Id)).Returns(allProducts[i]);
            }

            var response = await _client.DeleteAsync("/api/Admin/deleteProduct?id=" + Guid.NewGuid());
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task AddNewProduct()
        {
            var productToAdd = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductDescription = "Test",
                ProductImage = "Test",
                ProductName = "Test",
                ProductPrice = 50,
                ProductRating = 4.5
            };

            var content = new StringContent(JsonConvert.SerializeObject(productToAdd), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Admin/addProduct", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task EditProduct_ValidData_ReturnsOk()
        {

            var allProducts = new Product[]
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test",
                    ProductDescription = "Test",
                    ProductImage = "Test",
                    ProductPrice = 150,
                    ProductRating = 4.7
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductName = "Test2",
                    ProductDescription = "Test2",
                    ProductImage = "Test2",
                    ProductPrice = 150,
                    ProductRating = 4.7
                }
            };

            _factory.ProductRepositoryMock.Setup(p => p.GetAll()).Returns(allProducts);

            var dto = new ProductDto
            {
                ProductName = "Edited Product",
                ProductImage = "edited.jpg",
                ProductPrice = 150.00,
                ProductDescription = "Edited Description",
                ProductRating = 4.7
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/Admin/EditProduct?id={allProducts[0].Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        
    }
}
