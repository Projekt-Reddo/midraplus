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
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly IGrpcBoardClient _boardClient;
        private readonly IGrpcSignInClient _signInClient;

        public AuthController(IUserRepo userRepo, IJwtGenerator jwtGenerator, IMapper mapper, IGrpcBoardClient boardClient, IGrpcSignInClient signInClient)
        {
            _userRepo = userRepo;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
            _boardClient = boardClient;
            _signInClient = signInClient;
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
                #region BoardGrpc 
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
                #endregion

                // create claims and token for return to client
                var claims = _jwtGenerator.GenerateClaims(user);
                var token = _jwtGenerator.GenerateJwtToken(claims);

                var userToReturn = _mapper.Map<AuthDto>(user);
                userToReturn.AccessToken = token;

                #region SignInGrpc
                var signInCountStatus = _signInClient.AddSignIn(new AdminService.SignInCreateRequest());
                if (!signInCountStatus!.Status)
                {
                    BadRequest(new ResponseDto(400, "Cannot create new SignInCount"));
                }
                #endregion

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