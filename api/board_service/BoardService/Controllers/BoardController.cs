using AutoMapper;
using BoardService.Data;
using BoardService.Dtos;
using BoardService.Models;
using BoardService.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace BoardService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepo _boardRepo;
        private readonly IMapper _mapper;
        private readonly IGrpcUserClient _grpcUserClient;
        public BoardController(IBoardRepo boardRepo, IMapper mapper, IGrpcUserClient grpcUserClient)
        {
            _boardRepo = boardRepo;
            _mapper = mapper;
            _grpcUserClient = grpcUserClient;
        }

        [HttpGet]
        public async Task<ActionResult<BoardReadDto>> GetUserBoards(string userId)
        {
            var user = _grpcUserClient.GetUser(userId);

            if (user == null)
            {
                return NotFound(new ResponseDto(404, "User not found"));
            }
            // Get all boards of user
            var filter = Builders<Board>.Filter.Eq("UserId", userId);

            var boardsFromRepo = await _boardRepo.FindManyAsync(filter: filter);

            var boardForListDto = _mapper.Map<IEnumerable<BoardForListDto>>(boardsFromRepo);

            return Ok(boardForListDto);
            
        }

        /// <summary>
        /// Get all boars of that user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of boards</returns>
        [HttpGet("{userId:length(24)}")]
        public async Task<ActionResult<BoardReadDto>> GetUserBoards(string userId)
        {
            var user = _grpcUserClient.GetUser(userId);

            if (user == null)
            {
                return NotFound(new ResponseDto(404, "User not found"));
            }

            // Get all boards of user
            var filter = Builders<Board>.Filter.Eq("UserId", userId);

            (_, var boardsFromRepo) = await _boardRepo.FindManyAsync(filter: filter);

            var boardsReadtDto = _mapper.Map<IEnumerable<BoardReadDto>>(boardsFromRepo);

            return Ok(boardsReadtDto);
        }

        /// <summary>
        /// Add new board to database
        /// </summary>
        /// <param name="boardCreateDto">user info for creation of board</param>
        /// <returns>200 / 400 / 404</returns>
        [HttpPost]
        public async Task<ActionResult<ResponseDto>> AddBoard([FromBody] BoardCreateDto boardCreateDto)
        {
            // Validate input userId
            if (boardCreateDto.UserId == null)
            {
                return BadRequest(new ResponseDto(400, "UserId is required"));
            }

            var user = _grpcUserClient.GetUser(boardCreateDto.UserId);

            if (user == null)
            {
                return NotFound(new ResponseDto(404, "User not found"));
            }

            // Create new chat room & board
            var createdBoard = await _boardRepo.AddOneAsync(new Board
            {
                UserId = boardCreateDto.UserId,
                Name = user.Name,
            });

            return Ok(new ResponseDto(200, "Board created"));
        }

        /// <summary>
        /// Delete an board by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 / 404</returns>
        [HttpDelete("{id}")]
        // [AuthResourceAttribute(ResourceType = Constant.AuthResourceType.Board)]
        public async Task<ActionResult<ResponseDto>> DeleteBoard(string id)
        {
            var rs = await _boardRepo.DeleteOneAsync(id);

            if (rs == false)
            {
                return BadRequest(new ResponseDto(404, "Board not found"));
            }

            return Ok(new ResponseDto(200, "Board deleted"));
        }

        /// <summary>
        /// Update an board name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 / 404</returns>
        [HttpPut("{id}")]
        // [AuthResourceAttribute(ResourceType = Constant.AuthResourceType.Board)]
        public async Task<ActionResult<ResponseDto>> UpdateBoardName(string id, [FromBody] BoardUpdateDto boardUpdateDto)
        {
            var board = await _boardRepo.FindOneAsync(Builders<Board>.Filter.Eq("Id", id));
            if (board == null)
            {
                return NotFound(new ResponseDto(400, "Board not found"));
            }
            board.Name = boardUpdateDto.Name;
            var rs = await _boardRepo.UpdateOneAsync(id, board);

            if (rs == false)
            {
                return BadRequest(new ResponseDto(404, "Change board name failed"));
            }

            return Ok(new ResponseDto(200, "Board Name Updated"));
        }
    }
}