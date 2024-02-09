using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;


namespace WatchStore.Service
{
    public interface IUserService
    {
        User addNewUser(User u);
        User GetUserByEmail(String email);
        User GetUserById(Guid id);
    }
}
