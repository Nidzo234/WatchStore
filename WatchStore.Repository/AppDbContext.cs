using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStore.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { set; get; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCards { get; set; }
        public virtual DbSet<ProductInShoppingCart> ProductInShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ProductInShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.CurrnetProduct)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ProductInShoppingCart>()
                .HasOne(z => z.UserCart)
                .WithMany(z => z.ProductInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ShoppingCart>()
                .HasOne<User>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<ProductInOrder>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Product)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.ProductId);

            builder.Entity<ProductInOrder>()
                .HasOne(z => z.Order)
                .WithMany(z => z.ProductInOrders)
                .HasForeignKey(z => z.OrderId);
        }

    }
}
