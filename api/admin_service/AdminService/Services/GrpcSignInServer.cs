using AdminService.Data;
using AdminService.Models;
using AutoMapper;
using Grpc.Core;
using MongoDB.Driver;
using AdminService;

namespace AdminService.Services
{
    public class GrpcSignInServer : GrpcSignIn.GrpcSignInBase
    {
        private readonly ISignInRepo _repository;
        private readonly IMapper _mapper;

        public GrpcSignInServer(ISignInRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override async Task<SignInCreateResponse> AddSignIn(SignInCreateRequest request, ServerCallContext context)
        {
            DateTime currentDay = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day);
            var filter = Builders<SignIn>.Filter.Eq("At", currentDay);
            var sigin = await _repository.FindOneAsync(filter);

            if (sigin == null) // if there is no signin count for this day. Create new
            {
                var createdSignIn = await _repository.AddOneAsync(new SignIn()
                {
                    At = currentDay,
                    Times = 1
                });
                return new SignInCreateResponse
                {
                    Status = createdSignIn != null ? true : false
                };
            }
            else
            { // increase count +1 
                sigin.Times += 1;
                var rs = await _repository.UpdateOneAsync(sigin.Id, sigin);
                return new SignInCreateResponse
                {
                    Status = rs
                };
            }
        }

        public override Task<SignInUpdateResponse> UpdateSignIn(SignInUpdateRequest request, ServerCallContext context)
        {
            // Not implemented yet
            return base.UpdateSignIn(request, context);
        }
    }
}