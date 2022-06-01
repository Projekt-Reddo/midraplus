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

        public GrpcUserServer(IUserRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<UserGrpc> GetUser(GetUserRequest request, ServerCallContext context)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, request.Id);

            var user = await _repository.FindOneAsync(filter);

            return _mapper.Map<UserGrpc>(user);
        }
    }
}