using AccountService.Models;

namespace AccountService.Data
{
    public interface ISignInRepo : IRepository<SignIn>
    {
    }

    public class SignInRepo : Repository<SignIn>, ISignInRepo
    {
        public SignInRepo(IMongoContext mongoContext) : base(mongoContext) { }
    }
}