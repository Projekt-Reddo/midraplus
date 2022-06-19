using AutoMapper;
using BoardService;
using Grpc.Net.Client;
using Newtonsoft.Json;

namespace DrawService.Services
{
    public interface IGrpcBoardClient
    {
        Task<bool> IsUserOwnBoard(string boardId, string userId);
        Task<bool> SaveBoardData(string boardId, ICollection<ShapeGrpc> shapes, ICollection<NoteGrpc> notes);
    }

    public class GrpcBoardClient : IGrpcBoardClient
    {
        private readonly GrpcBoard.GrpcBoardClient _client;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcBoardClient> _logger;

        public GrpcBoardClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcBoardClient> logger)
        {
            // Injection
            _mapper = mapper;
            _logger = logger;

            // Get url from appsettings.json
            string boardServerUrl = configuration.GetValue<string>("Grpc:Board");

            // Create Grpc client
            var channel = GrpcChannel.ForAddress(boardServerUrl);
            _client = new GrpcBoard.GrpcBoardClient(channel);
        }

        public async Task<bool> IsUserOwnBoard(string boardId, string userId)
        {
            var request = new IsUserOwnBoardRequest
            {
                BoardId = boardId,
                UserId = userId
            };

            var response = await _client.IsUserOwnBoardAsync(request);

            return response.Status;
        }

        public async Task<bool> SaveBoardData(string boardId, ICollection<ShapeGrpc> shapes, ICollection<NoteGrpc> notes)
        {
            var request = new BoardDataRequest
            {
                BoardId = boardId
            };

            request.Shapes.AddRange(shapes);
            request.Notes.AddRange(notes);

            var response = await _client.SaveBoardDataAsync(request);

            _logger.LogInformation($"Saved board - {boardId} with status {JsonConvert.SerializeObject(response)}");

            return response.Status;
        }
    }
}