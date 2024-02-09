using WatchStore.Domain;

namespace WatchStore.Repository
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(Guid id);
        void Update(User user);
    }
}
