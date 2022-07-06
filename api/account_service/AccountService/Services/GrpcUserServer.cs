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
    }
}