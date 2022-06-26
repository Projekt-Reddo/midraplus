using AccountService.Data;
using AccountService.Dtos;
using AccountService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get Users with pagination 
        /// </summary>
        /// <returns>List of user and total User</returns>
        [HttpGet("")]
        public async Task<ActionResult<PaginationResponse<IEnumerable<UserManageListDto>>>> GetAllUserManage([FromQuery] PaginationParameterDto pagination)
        {
            // Filter User Account
            var userFilter = Builders<User>.Filter.Eq("IsAdmin", false) | Builders<User>.Filter.Eq("IsAdmin", BsonNull.Value);

            if (pagination.SearchName != null)
            {
                userFilter = userFilter & Builders<User>.Filter.Regex("Name", new BsonRegularExpression(pagination.SearchName, "i"));
            }

            var skipPage = (pagination.PageNumber - 1) * pagination.PageSize;

            // Get total User
            (var totalUser, _) = (await _userRepo.FindManyAsync(filter: userFilter));

            (_, var usersFromRepo) = await _userRepo.FindManyAsync(filter: userFilter, limit: pagination.PageSize, skip: skipPage);

            var users = _mapper.Map<IEnumerable<UserManageListDto>>(usersFromRepo);

            return Ok(new PaginationResponse<IEnumerable<UserManageListDto>>((Int32)totalUser, users));
        }

        [HttpPut("ban/{userId:length(24)}")]
        public async Task<ActionResult> BanUser(string userId)
        {
            var userFilter = Builders<User>.Filter.Eq("Id", userId);
            var user = await _userRepo.FindOneAsync(userFilter);

            if (user == null)
            {
                return NotFound(new ResponseDto(404, "User not found"));
            }

            user.IsBanned = !user.IsBanned;

            var rs = await _userRepo.UpdateOneAsync(userId, user);

            if (rs == false)
            {
                return BadRequest(new ResponseDto(400, "Fail to ban user"));
            }

            return Ok();
        }
    }
}