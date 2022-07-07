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

        public IMessageBusPublisher _messageBusPublisher { get; }
        public ILogger<AuthController> _logger { get; }

        public AuthController(IUserRepo userRepo, IJwtGenerator jwtGenerator, IMapper mapper,
            IMessageBusPublisher messageBusPublisher,
            ILogger<AuthController> logger
            )
        {
            _userRepo = userRepo;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
            _messageBusPublisher = messageBusPublisher;
            _logger = logger;
        }

        /// <summary>
        /// Login and sign up with google
        /// </summary>
        /// <param name="userView">user info for login/signup for google account</param>
        /// <returns>200 / 400 / 404</returns>
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] UserView userView)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings());
                (var user, var isNew) = await _userRepo.Authenticate(payload);
                #region Create default board Asynchronous Message Queue
                if (isNew)
                {
                    try // publish create board message to rabbitmq
                    {
                        var createBoardEventDto = new MessageCreateBoardPublishDto
                        {
                            Id = user.Id,
                            Name = $"Default {user.Name} board",
                            Event = "CreateBoard"
                        };
                        _messageBusPublisher.PublishCreateBoard(createBoardEventDto);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex.Message, "Error when publish add default board to message queue");
                    }
                }
                #endregion

                // create claims and token for return to client
                var claims = _jwtGenerator.GenerateClaims(user);
                var token = _jwtGenerator.GenerateJwtToken(claims);

                var userToReturn = _mapper.Map<AuthDto>(user);
                userToReturn.AccessToken = token;

                #region AddSignIn Asynchronous Message Queue
                try // publish add sign in message to rabbitmq
                {
                    var addSignInEventDto = new MessageAddSiginPublishDto
                    {
                        Event = "AddSignIn"
                    };
                    _messageBusPublisher.PublishAddSignIn();
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.Message, "Error when publish add SignIn to message queue");
                }


                #endregion

                return Ok(
                    userToReturn
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}