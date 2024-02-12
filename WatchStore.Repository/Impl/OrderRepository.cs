using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;


namespace WatchStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private DbSet<Order> entities;

        public OrderRepository (AppDbContext context)
        {
            _context = context;
            entities = context.Set<Order>();
        }
        public List<Order> getAllOrdersFromUser(Guid userId)
        {
            string id = userId.ToString();
            var res = entities.Where(s => s.UserId == id).
                Include(z => z.ProductInOrders)
               .Include("ProductInOrders.Product").ToListAsync().Result; 
            return res;
        }

        public List<Order> getAllOrders()
        {
            return entities
               .Include(z => z.User)
               .Include(z => z.ProductInOrders)
               .Include("ProductInOrders.Product")
               .ToListAsync().Result;
        }
    }
}
