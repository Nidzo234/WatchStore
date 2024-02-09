using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;
using WatchStore.Repository;

namespace WatchStore.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User addNewUser(User u)
        {
            return _userRepository.Create(u);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public User GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }
    }
}
