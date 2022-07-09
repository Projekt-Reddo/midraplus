using AutoMapper;
using AdminService.Models;
using Grpc.Net.Client;
using UserService;

namespace AdminService.Services
{
    public interface IGrpcUserClient
    {
        User? GetUser(string id);
        TotalAccountRespone GetTotalAccount();
    }

    public class GrpcUserClient : IGrpcUserClient
    {

        private readonly IMapper _mapper;
        private readonly ILogger<GrpcUserClient> _logger;
        private readonly string _accountServiceUrl;
        private readonly GrpcUser.GrpcUserClient _client;

        public GrpcUserClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcUserClient> logger)
        {
            _mapper = mapper;
            _logger = logger;

            _accountServiceUrl = configuration.GetValue<string>("Grpc:User");

            var channel = GrpcChannel.ForAddress(_accountServiceUrl);
            _client = new GrpcUser.GrpcUserClient(channel);
        }

        public User? GetUser(string id)
        {
            _logger.LogInformation($"Getting user from Account Service ({_accountServiceUrl})");

            var request = new GetUserRequest { Id = id };

            try
            {
                var response = _client.GetUser(request);

                return _mapper.Map<User>(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fail to get User from Account Service");
                return null;
            }
        }
        public TotalAccountRespone GetTotalAccount()
        {
            _logger.LogInformation($"Getting total accounts from Account Service ({_accountServiceUrl})");

            var request = new GetTotalAccountRequest {  };
            try
            {
                var response = _client.GetTotalAccount(request);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Fail to get Total Accounts from Account Service");
                return null;
            }
        }
    }
}