using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.Api.Tokens.Dto;
using PimpMyRide.Core.Tokens;
using PimpMyRide.Core.Tokens.Dto;

namespace PimpMyRide.Core.Api.Tokens
{
    [Route("api/token")]
    [AllowAnonymous]
    public class TokenController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public TokenController(ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [ProducesResponseType(typeof(Jwt), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tokenDto = _mapper.Map<TokenDto>(dto);

            if (!await _tokenService.CheckToken(tokenDto, cancellationToken))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            var newToken = await _tokenService.RefreshSecurityToken(tokenDto, cancellationToken);
            var response = new ApiOkResponse(newToken);
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDto dto, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tokenDto = _mapper.Map<TokenDto>(dto);

            if (!await _tokenService.CheckToken(tokenDto, cancellationToken))
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            await _tokenService.RevokeToken(tokenDto, cancellationToken);
            return NoContent();
        }
    }
}
