using AutoMapper;
using BoardService;
using Grpc.Net.Client;

namespace AccountService.Services
{
    public interface IGrpcBoardClient
    {
        BoardCreateResponse? AddBoard(BoardCreateRequest boardCreateRequest);
    }

    public class GrpcBoardClient : IGrpcBoardClient
    {

        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcBoardClient> _logger;
        private readonly GrpcBoard.GrpcBoardClient _client;

        public GrpcBoardClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcBoardClient> logger)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;

            string boardServerUrl = _configuration.GetValue<string>("Grpc:Board");

            var channel = GrpcChannel.ForAddress(boardServerUrl);
            _client = new GrpcBoard.GrpcBoardClient(channel);

            _logger.LogInformation($"Connected to Board Server with {boardServerUrl}");
        }

        public BoardCreateResponse? AddBoard(BoardCreateRequest boardCreateRequest)
        {

            try
            {
                BoardCreateResponse response = _client.AddBoard(boardCreateRequest);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail create Board from BoardServer");
                return null;
            }
        }
    }
}