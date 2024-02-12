using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStoreTest.IntegrationTests.ControllerTests
{
    public class AdminControllerTests
        //: IDisposable
    {
        /*
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
        }*/
        /*

        [Fact]
        public async Task get_AllProducts()
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
                    ProductPrice = 140,
                    ProductRating = 4.7
                }
            };

            _factory.ProductRepositoryMock.Setup(p => p.GetAll()).Returns(allProducts);
            
            var response = await _client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var data = JsonConvert.DeserializeObject<IEnumerable<Product>>(await response.Content.ReadAsStringAsync());


            Assert.Collection((data as IEnumerable<Product>),
                r => {
                    Assert.Equal(allProducts[0].Id, r.Id);
                    Assert.Equal("Test", r.ProductName);
                    Assert.Equal(150, r.ProductPrice);
                },
                r =>
                {
                    Assert.Equal(allProducts[1].Id, r.Id);
                    Assert.Equal("Test2", r.ProductName);
                    Assert.Equal(140, r.ProductPrice);
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
        */


        [Fact]
        public async Task AddPRoductRequest_AddProductToDatabase()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto{
                id = Guid.NewGuid(),
                ProductName = "test1",
                ProductDescription = "test1",
                ProductImage = "test1",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task AddPRoductRequest_InvalidData_AddProductToDatabase()
        {
            var application = new CustomWebApplicationFactory();
            Product p = null;
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", p);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddPRoductRequest_EmptyDto_AddProductToDatabase()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto p = new ProductDto();
            var client = application.CreateClient();

            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", p);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProductRequest_ReturnsAllProductsFromDatabase()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product1 = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test2",
                ProductDescription = "test2",
                ProductImage = "test2",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            ProductDto product2 = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test3",
                ProductDescription = "test3",
                ProductImage = "test3",
                ProductPrice = 200,
                ProductRating = 4.5
            };

            var client = application.CreateClient();

            var response1 = await client.PostAsJsonAsync("/api/Admin/addProduct", product1);
            response1.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

            var response2 = await client.PostAsJsonAsync("/api/Admin/addProduct", product2);
            response2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);


            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);

            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Assert.Equal(2, data.Count);
            Assert.Collection((data as IEnumerable<Product>),
                r => {
                    Assert.Equal("test2", r.ProductName);
                    Assert.Equal(100, r.ProductPrice);
                },
                r =>
                {
                    Assert.Equal("test3", r.ProductName);
                    Assert.Equal(200, r.ProductPrice);
                });

        }


        [Fact]
        public async Task GetAllProductRequest_EmptyDB_ReturnsNotFound()
        {
            var application = new CustomWebApplicationFactory();
            var client = application.CreateClient();


            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.NotFound, response3.StatusCode);

        }



        [Fact]
        public async Task GetAllProductRequest_OnlyOneProductInDB_ReturnsOneProductsFromDatabase()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product1 = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test2",
                ProductDescription = "test2",
                ProductImage = "test2",
                ProductPrice = 100,
                ProductRating = 4.4
            };
           

            var client = application.CreateClient();

            var response1 = await client.PostAsJsonAsync("/api/Admin/addProduct", product1);
            response1.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response1.StatusCode);

          

            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);

            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Assert.Equal(1, data.Count);
            Assert.Collection((data as IEnumerable<Product>),
                r =>
                {
                    Assert.Equal("test2", r.ProductName);
                    Assert.Equal(100, r.ProductPrice);
                });
            

        }


        [Fact]
        public async Task DeleteProductRequest_DeletesProductFromDatabase()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Guid firstProductId = Guid.NewGuid();
            if (data != null)
            {
                firstProductId = data.FirstOrDefault()?.Id ?? Guid.Empty; // Assuming Id is of type Guid, Guid.Empty is a default value if the list is empty
                Console.WriteLine("ID of the first product: " + firstProductId);
            }

  

            var url = "/api/Admin/deleteProduct?id=" + firstProductId;

            var response2 = await client.DeleteAsync(url);
            response2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);
        }


        [Fact]
        public async Task DeleteProductRequest_InvalidID()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();


            var url = "/api/Admin/deleteProduct?id=" + Guid.NewGuid();

            var response2 = await client.DeleteAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }


        [Fact]
        public async Task editProduct_ValidData_ReturnsOK()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };

            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);



            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Guid firstProductId = Guid.NewGuid();
            if (data != null)
            {
                firstProductId = data.FirstOrDefault()?.Id ?? Guid.Empty; // Assuming Id is of type Guid, Guid.Empty is a default value if the list is empty
                Console.WriteLine("ID of the first product: " + firstProductId);
            }

            var dto = new ProductDto
            {
                ProductName = "Edited Product",
                ProductImage = "edited.jpg",
                ProductPrice = 150.00,
                ProductDescription = "Edited Description",
                ProductRating = 4.7
            };



            var url = "/api/Admin/EditProduct?id=" + firstProductId.ToString();

            var response2 = await client.PostAsJsonAsync(url, dto);
            response2.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response2.StatusCode);

        }



        [Fact]
        public async Task editProduct_InvalidData_ReturnsNotFound()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);



            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();


            Guid firstProductId = Guid.NewGuid();
            if (data != null)
            {
                firstProductId = data.FirstOrDefault()?.Id ?? Guid.Empty; // Assuming Id is of type Guid, Guid.Empty is a default value if the list is empty
                Console.WriteLine("ID of the first product: " + firstProductId);
            }


            var dto = new ProductDto
            {
                ProductName = "Edited Product",
                ProductImage = "edited.jpg",
                ProductPrice = 150.00,
                ProductDescription = "Edited Description",
                ProductRating = 4.7
            };


            var url = "/api/Admin/EditProduct?id=" + Guid.NewGuid();

            var response2 = await client.PostAsJsonAsync(url, dto);
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
        }

        [Fact]
        public async Task GetProduct_ProductExists_ReturnsProduct()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Guid firstProductId = Guid.NewGuid();
            if (data != null)
            {
                firstProductId = data.FirstOrDefault()?.Id ?? Guid.Empty;
                Console.WriteLine("ID of the first product: " + firstProductId);
            }

            var response4 = await client.GetAsync("/api/Admin/getProduct?id=" + firstProductId.ToString());
            Assert.Equal(HttpStatusCode.OK, response4.StatusCode);


        }


        [Fact]
        public async Task GetProduct_ProductNotExists_ReturnsNotFound()
        {
            var application = new CustomWebApplicationFactory();
            ProductDto product = new ProductDto
            {
                id = Guid.NewGuid(),
                ProductName = "test4",
                ProductDescription = "test4",
                ProductImage = "test4",
                ProductPrice = 100,
                ProductRating = 4.4
            };
            var client = application.CreateClient();
            var response = await client.PostAsJsonAsync("/api/Admin/addProduct", product);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            var response3 = await client.GetAsync("/api/Admin/getAllProducts");
            Assert.Equal(HttpStatusCode.OK, response3.StatusCode);
            var data = await response3.Content.ReadFromJsonAsync<List<Product>>();

            Guid firstProductId = Guid.NewGuid();
            if (data != null)
            {
                firstProductId = data.FirstOrDefault()?.Id ?? Guid.Empty;
                Console.WriteLine("ID of the first product: " + firstProductId);
            }

            var response4 = await client.GetAsync("/api/Admin/getProduct?id=" + Guid.NewGuid());
            Assert.Equal(HttpStatusCode.NotFound, response4.StatusCode);


        }

        
    }
}
