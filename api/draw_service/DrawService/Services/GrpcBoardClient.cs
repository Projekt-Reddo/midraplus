using AutoMapper;
using BoardService;
using DrawService.Dtos;
using DrawService.Models;
using Grpc.Net.Client;
using Newtonsoft.Json;

namespace DrawService.Services
{
    public interface IGrpcBoardClient
    {
        /// <summary>
        /// Clear all shapes of that board
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        Task<bool> ClearBoard(string boardId);

        /// <summary>
        /// Check request user is the owner of that board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> IsUserOwnBoard(string boardId, string userId);

        /// <summary>
        /// Save shapes and notes to that board
        /// </summary>
        /// <param name="boardId"></param>
        /// <param name="shapes"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        Task<bool> SaveBoardData(string boardId, ICollection<ShapeGrpc> shapes, ICollection<NoteGrpc> notes);

        /// <summary>
        /// Load all shapes of that board from Board service
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        Task<BoardReadDto> LoadBoardData(string boardId);
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

        public async Task<bool> ClearBoard(string boardId)
        {
            var request = new ClearBoardRequest
            {
                BoardId = boardId
            };

            var response = await _client.ClearBoardAsync(request);

            return response.Status;
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

        public async Task<BoardReadDto> LoadBoardData(string boardId)
        {
            var request = new BoardLoadDataRequest
            {
                BoardId = boardId
            };

            var response = await _client.LoadBoardDataAsync(request);

            _logger.LogInformation($"Load board - {boardId} with status {JsonConvert.SerializeObject(response)}");

            var returnedBoard = _mapper.Map<BoardReadDto>(response);

            return returnedBoard;
        }
    }
}