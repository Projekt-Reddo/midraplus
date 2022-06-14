using AccountService.Data;
using AccountService.Dtos;
using AccountService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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