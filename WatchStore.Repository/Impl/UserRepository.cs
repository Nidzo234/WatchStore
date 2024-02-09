using WatchStore.Domain;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;



namespace WatchStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
           
            return user;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(Guid id)
        {
            return _context
                .Users
                .Include(z => z.UserCart)
                .Include("UserCart.ProductInShoppingCarts")
                .Include("UserCart.ProductInShoppingCarts.CurrnetProduct")
                .SingleOrDefault(s => s.Id == id);
        }



        public void Update(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("entity");
            }
            _context.Users.Update(user);
            _context.SaveChanges();
        }

      
    }
}
