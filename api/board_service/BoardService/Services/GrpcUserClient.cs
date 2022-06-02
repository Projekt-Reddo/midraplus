using AutoMapper;
using BoardService.Models;
using Grpc.Net.Client;
using UserService;

namespace BoardService.Services
{
    public interface IGrpcUserClient
    {
        User? GetUser(string id);
    }

    public class GrpcUserClient : IGrpcUserClient
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcUserClient> _logger;
        private readonly GrpcUser.GrpcUserClient _client;

        public GrpcUserClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcUserClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            string userServerUrl = _configuration.GetValue<string>("Grpc:User");

            var channel = GrpcChannel.ForAddress(userServerUrl);
            _client = new GrpcUser.GrpcUserClient(channel);

            _logger.LogInformation($"Connected to User Server with {userServerUrl}");
        }

        public User? GetUser(string id)
        {
            var request = new GetUserRequest { Id = id };

            try
            {
                var response = _client.GetUser(request);

                return _mapper.Map<User>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to get User from UserServer");
                return null;
            }
        }
    }
}