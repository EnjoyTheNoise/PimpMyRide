using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.Api.Infrastructure.Consts;
using PimpMyRide.Core.Tokens;
using PimpMyRide.Core.Tokens.Dto;
using PimpMyRide.Core.Users;
using PimpMyRide.Core.Users.Dto;

namespace PimpMyRide.Core.Api.Users
{
    [Route("api/user")]
    [AllowAnonymous]
    [Controller]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserController(ITokenService tokenService, IUserService userService, IMapper mapper)
        {
            _tokenService = tokenService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(Jwt), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequestDto,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var validateResult = _userService.CredentialsValid(loginRequestDto);
            if (validateResult)
            {
                var token = await _tokenService.CreateToken(_mapper.Map<TokenCreateDto>(loginRequestDto),
                    cancellationToken);
                if (token != null) return Ok(new ApiOkResponse(token));
            }

            var errorMessage = ApiResponse.CreateErrorMessage(ErrorMessages.WrongCredentials, ErrorMessages.DefaultKey);
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, errorMessage));
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _tokenService.DeactivateToken(cancellationToken);
            return NoContent();
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegisterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDto registerRequestDto,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_userService.UserExist(registerRequestDto.Email))
            {
                var errorMessage = ApiResponse.CreateErrorMessage(ErrorMessages.EmailExists, ErrorMessages.EmailKey);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, errorMessage));
            }

            var registerResponseDto = await _userService.CreateUserAsync(registerRequestDto, cancellationToken);
            return Ok(new ApiOkResponse(registerResponseDto));
        }
    }
}
