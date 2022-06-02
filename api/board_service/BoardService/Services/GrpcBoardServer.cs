using Grpc.Core;
using AutoMapper;
using BoardService.Data;
using BoardService;
using BoardService.Models;

namespace BoardService.Services
{
    public class GrpcBoardServer : GrpcBoard.GrpcBoardBase
    {
        private readonly IBoardRepo _boardRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcBoardServer> _logger;

        public GrpcBoardServer(IConfiguration configuration, IMapper mapper, ILogger<GrpcBoardServer> logger,
        IBoardRepo boardRepo)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
            _boardRepo = boardRepo;
        }

        public override async Task<BoardCreateResponse> AddBoard(BoardCreateRequest request, ServerCallContext context)
        {
            // Create new board
            var createdBoard = await _boardRepo.AddOneAsync(new Board
            {
                UserId = request.UserId,
                Name = request.Name,
            });
            return new BoardCreateResponse
            {
                Status = createdBoard != null ? true : false
            };
        }
    }
}