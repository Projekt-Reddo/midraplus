using AccountService.Models;

namespace AccountService.Data
{
    public interface IUserRepo : IRepository<User> { }

    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(IMongoContext context) : base(context) { }
    }
}