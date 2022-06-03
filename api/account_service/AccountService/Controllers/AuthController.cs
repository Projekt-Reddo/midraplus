using AccountService.Data;
using AccountService.Models;
using AccountService.Helpers;
using AccountService.Dtos;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Google.Apis.Auth;
using MongoDB.Driver;
using AccountService.Services;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ISignInRepo _signInRepo;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly IGrpcBoardClient _boardClient;

        public AuthController(IUserRepo userRepo, ISignInRepo signInRepo, IJwtGenerator jwtGenerator, IMapper mapper,
        IGrpcBoardClient boardClient
        )
        {
            _userRepo = userRepo;
            _signInRepo = signInRepo;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
            _boardClient = boardClient;
        }
        /// <summary>
        /// Login and sign up with google
        /// </summary>
        /// <param name="userView">user info for login/signup for google account</param>
        /// <returns>200 / 400 / 404</returns>
        [HttpPost]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] UserView userView)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings());
                (var user, var isNew) = await _userRepo.Authenticate(payload);
                if (isNew)
                {
                    // Call AddBoard service from grpc board client
                    var status = _boardClient.AddBoard(new BoardService.BoardCreateRequest
                    {
                        UserId = user.Id,
                        Name = $"Default {user.Name}"
                    });
                    if (!status!.Status)
                    {
                        BadRequest(new ResponseDto(400, "Cannot create board"));
                    }
                }

                // create claims and token for return to client
                var claims = _jwtGenerator.GenerateClaims(user);
                var token = _jwtGenerator.GenerateJwtToken(claims);

                var userToReturn = _mapper.Map<AuthDto>(user);
                userToReturn.AccessToken = token;

                DateTime currentDay = new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day);
                var filter = Builders<SignIn>.Filter.Eq("At", currentDay);
                var sigin = await _signInRepo.FindOneAsync(filter);
                if (sigin == null)
                {
                    await _signInRepo.AddOneAsync(new SignIn()
                    {
                        At = currentDay,
                        Times = 1
                    });
                }
                else
                {
                    sigin.Times += 1;
                    await _signInRepo.UpdateOneAsync(sigin.Id, sigin);
                }
                return Ok(
                    userToReturn
                );
            }
            catch (Exception ex)
            {
                BadRequest(new ResponseDto(400, ex.Message));
            }
            return BadRequest(new ResponseDto(400));
        }
    }
}