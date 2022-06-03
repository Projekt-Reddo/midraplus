using AccountService.Models;
using MongoDB.Driver;

namespace AccountService.Data
{
    public interface IUserRepo : IRepository<User>
    {
        Task<(User, bool)> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
    }

    public class UserRepo : Repository<User>, IUserRepo
    {
        public UserRepo(IMongoContext context) : base(context)
        {
        }

        public async Task<(User, bool)> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            await Task.Delay(1);
            return await this.FindUserOrAdd(payload);
        }
        private async Task<(User, bool)> FindUserOrAdd(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {

            // find user
            var u = await FindOneAsync(Builders<User>.Filter.Eq("Email", payload.Email));
            var isNew = false;
            if (u == null)
            {
                isNew = true;
                u = new User()
                {
                    Name = payload.Name,
                    Email = payload.Email,
                    Avatar = payload.Picture,
                    Issuer = payload.Issuer
                };
                // add user
                await AddOneAsync(u);
            }
            return (u, isNew);
        }
    }
}