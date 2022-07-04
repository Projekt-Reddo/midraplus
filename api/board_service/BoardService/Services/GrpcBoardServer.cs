using Grpc.Core;
using AutoMapper;
using BoardService.Data;
using BoardService.Models;
using MongoDB.Driver;
using BoardService.Dtos;

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

        public override async Task<ClearBoardResponse> ClearBoard(ClearBoardRequest request, ServerCallContext context)
        {
            var boardFromRepo = await _boardRepo.FindOneAsync(Builders<Board>.Filter.Eq(b => b.Id, request.BoardId));

            if (boardFromRepo == null)
            {
                return new ClearBoardResponse
                {
                    Status = false,
                    Message = "Board id not found"
                };
            }

            boardFromRepo.Shapes = null!;
            boardFromRepo.Notes = null!;

            var rs = await _boardRepo.UpdateOneAsync(request.BoardId, boardFromRepo);

            if (rs == false)
            {
                return new ClearBoardResponse()
                {
                    Status = false,
                    Message = "Clear board fail!!!"
                };
            }

            return new ClearBoardResponse()
            {
                Status = false,
                Message = "All Shapes and Notes were cleared"
            };
        }

        public override async Task<IsUserOwnBoardResponse> IsUserOwnBoard(IsUserOwnBoardRequest request, ServerCallContext context)
        {
            var boardFromRepo = await _boardRepo.FindOneAsync(Builders<Board>.Filter.Eq(b => b.Id, request.BoardId));

            if (boardFromRepo == null)
            {
                return new IsUserOwnBoardResponse
                {
                    Status = false,
                    Message = "Board id not found"
                };
            }

            if (boardFromRepo.UserId == request.UserId)
            {
                return new IsUserOwnBoardResponse()
                {
                    Status = true,
                    Message = "User is owner of board"
                };
            }

            return new IsUserOwnBoardResponse()
            {
                Status = false,
                Message = "User is not owner of board"
            };
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

        public override async Task<BoardDataResponse> SaveBoardData(BoardDataRequest request, ServerCallContext context)
        {
            var boardFromRepo = await _boardRepo.FindOneAsync(Builders<Board>.Filter.Eq(b => b.Id, request.BoardId));

            if (boardFromRepo == null)
            {
                return new BoardDataResponse
                {
                    Status = false,
                    Message = "Board id not found"
                };
            }

            // Add new shapes 
            var newShapes = _mapper.Map<List<Shape>>(request.Shapes);

            if (boardFromRepo.Shapes == null)
            {
                boardFromRepo.Shapes = new List<Shape>();
            }

            // Override old shapes
            boardFromRepo.Shapes = newShapes;

            // foreach (var shape in newShapes)
            // {
            //     boardFromRepo.Shapes.Add(shape);
            // }

            // Add new notes
            var newNotes = _mapper.Map<List<Note>>(request.Notes);

            if (boardFromRepo.Notes == null)
            {
                boardFromRepo.Notes = new List<Note>();
            }

            // Override old notes
            boardFromRepo.Notes = newNotes;

            // foreach (var note in newNotes)
            // {
            //     boardFromRepo.Notes.Add(note);
            // }

            // Update last edit
            boardFromRepo.LastEdit = DateTime.Now;

            // Save board
            var updatedBoard = await _boardRepo.UpdateOneAsync(request.BoardId, boardFromRepo);

            return new BoardDataResponse
            {
                Status = true,
                Message = "Board data saved"
            };
        }

        public override async Task<BoardLoadDataResponse> LoadBoardData(BoardLoadDataRequest request, ServerCallContext context)
        {
            Board boardFromRepo = await _boardRepo.FindOneAsync(Builders<Board>.Filter.Eq(b => b.Id, request.BoardId));

            BoardLoadDataResponse boardResult = _mapper.Map<BoardLoadDataResponse>(boardFromRepo);

            return boardResult;
        }
        public override async Task<BoardLoadByTimeResponse> LoadBoardListByTime(BoardLoadByTimeRequest request, ServerCallContext context)
        {
            BoardLoadByTime requestBoard = _mapper.Map<BoardLoadByTime>(request);

            var filter = Builders<Board>.Filter.Gte("CreatedAt", requestBoard.StartDate) & Builders<Board>.Filter.Lte("CreatedAt", requestBoard.EndDate);

            (_, IEnumerable<Board> boardFromrepo) = await _boardRepo.FindManyAsync(filter: filter);

            List<BoardLoadByTimeGrpc> boardToReturn = _mapper.Map<List<BoardLoadByTimeGrpc>>(boardFromrepo);

            var rs = new BoardLoadByTimeResponse();

            rs.BoardList.AddRange(boardToReturn);

            return rs;
        }
    }
}