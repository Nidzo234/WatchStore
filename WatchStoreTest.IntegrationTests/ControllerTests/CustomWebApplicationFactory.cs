using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Repository;

namespace WatchStoreTest.IntegrationTests.ControllerTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<IRepository<Product>> ProductRepositoryMock { get; }

        public CustomWebApplicationFactory ()
        {
            ProductRepositoryMock = new Mock<IRepository<Product>>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(ProductRepositoryMock.Object);

            });
        }
    }
}
