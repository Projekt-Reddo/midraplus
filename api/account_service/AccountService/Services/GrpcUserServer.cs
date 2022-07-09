using AccountService.Data;
using AccountService.Models;
using AutoMapper;
using Grpc.Core;
using MongoDB.Driver;
using UserService;

namespace AccountService.Services
{
    public class GrpcUserServer : GrpcUser.GrpcUserBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcUserServer> _logger;

        public GrpcUserServer(IUserRepo repository, IMapper mapper, ILogger<GrpcUserServer> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public override async Task<UserGrpc> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, request.Id);

            var user = await _repository.FindOneAsync(filter);

            _logger.LogInformation($"GetUser: {user.Id}");

            return _mapper.Map<UserGrpc>(user);
        }
        public override async Task<TotalAccountRespone> GetTotalAccount(GetTotalAccountRequest request, ServerCallContext context)
        {
            var total = await _repository.FindManyAsync();
<<<<<<< Updated upstream
            var filterNewMem = Builders<User>.Filter.Gt("CreatedAt", DateTime.Now.AddDays(-7));
            var total7 = await _repository.FindManyAsync(filterNewMem);
            TotalAccountRespone returnValue = new TotalAccountRespone
            {
                Total = Convert.ToInt32(total.total),
                Account7Days = Convert.ToInt32(total7.total),
=======
            TotalAccountRespone returnValue = new TotalAccountRespone
            {
                Total = Convert.ToInt32(total.total),
>>>>>>> Stashed changes
            };
            return returnValue;
        }
    }
}