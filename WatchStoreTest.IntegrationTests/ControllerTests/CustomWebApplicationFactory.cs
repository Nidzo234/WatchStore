using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
                //services.AddSingleton(ProductRepositoryMock.Object);
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                services.AddSqlServer<AppDbContext>("Server=(localdb)\\mssqllocaldb;Database=WatchStoreDBNova_test;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
                var dbContext = createDbContext(services);
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
                
            });
        }

        private static AppDbContext createDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return dbContext;

        }
    }
}
