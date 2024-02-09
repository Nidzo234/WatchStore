using Microsoft.EntityFrameworkCore;

using WatchStore.Service;
using Microsoft.Extensions.Configuration;
using WatchStoreApi;
using WatchStore.Repository;
using WatchStore.Service.Interface;
using WatchStore.Domain;
using Stripe;


public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var Configuration = builder.Configuration;
        // Add services to the container.
        builder.Services.AddCors();

        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WatchStoreDBNova;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"));

        builder.Services.AddTransient<IJwtService, JwtService>();
        builder.Services.AddTransient<IUserService, UserService>();
        builder.Services.AddTransient<IProductService, WatchStore.Service.ProductService>();
        builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();

        builder.Services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        var app = builder.Build();



        StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors(options => options
                        .WithOrigins(new[] { "http://localhost:3000", "http://localhost:8080", "http://localhost:4200" })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                    );

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}