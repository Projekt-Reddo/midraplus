using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminService;
using AutoMapper;
using BoardService;
using Grpc.Net.Client;

namespace AccountService.Services
{
    public interface IGrpcSignInClient
    {
        SignInCreateResponse? AddSignIn(SignInCreateRequest signInCreateRequest);
    }

    public class GrpcSignInClient : IGrpcSignInClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcSignInClient> _logger;
        private readonly GrpcSignIn.GrpcSignInClient _client;

        public GrpcSignInClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcSignInClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            string signInServer = _configuration.GetValue<string>("Grpc:Admin");

            var channel = GrpcChannel.ForAddress(signInServer);
            _client = new GrpcSignIn.GrpcSignInClient(channel);

            _logger.LogInformation($"Connected to SignIn Server with {signInServer}");
        }

        public SignInCreateResponse? AddSignIn(SignInCreateRequest signInCreateRequest)
        {
            try
            {
                SignInCreateResponse response = _client.AddSignIn(signInCreateRequest);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail create SignIn from SignInServer");
                return null;
            }
        }
    }
}