using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.Infrastructure.Options;
using PimpMyRide.Core.Tokens.Dto;
using PimpMyRide.Core.Users;

namespace PimpMyRide.Core.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _options;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public TokenService(IUnitOfWork unitOfWork, IUserService userService, IOptions<JwtOptions> options,
            ITokenManager tokenManager,
            IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _options = options.Value;
            _userService = userService;
            _tokenManager = tokenManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<Jwt> CreateToken(TokenCreateDto dto, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.UserRepository
                .Get(u => string.Equals(u.Email, dto.Email, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            if (!_userService.VerifyPassword(user, dto.Password))
            {
                return null;
            }

            var claims = CreateClaims(user);
            var refreshToken = BuildRefreshToken(user);
            var jwt = BuildJwt(claims);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            var guid = await _tokenManager.GetGuidFromRequest(cancellationToken);
            await _tokenManager.AddNewTokens(token, refreshToken, user.Id, guid, cancellationToken);

            return new Jwt
            {
                SecurityToken = token,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddMinutes(_options.Exp)
            };
        }

        public async Task<Jwt> RefreshSecurityToken(TokenDto dto, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Id == dto.UserId).FirstOrDefault();
            var claims = CreateClaims(user);
            var jwt = BuildJwt(claims);

            var newToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var guid = await _tokenManager.GetGuidFromRequest(cancellationToken);

            await _tokenManager.SetNewSecurityToken(newToken, dto.UserId, guid, cancellationToken);

            return new Jwt
            {
                SecurityToken = newToken,
                RefreshToken = dto.RefreshToken,
                ExpiryDate = DateTime.UtcNow.AddMinutes(_options.Exp)
            };
        }

        public async Task RevokeToken(TokenDto dto, CancellationToken cancellationToken)
        {
            var guid = await _tokenManager.GetGuidFromRequest(cancellationToken);
            await _tokenManager.RevokeTokens(dto.UserId, guid, cancellationToken);
        }

        public async Task<bool> CheckToken(TokenDto dto, CancellationToken cancellationToken)
        {
            var guid = await _tokenManager.GetGuidFromRequest(cancellationToken);
            var refreshToken = await _tokenManager.GetRefreshToken(dto.UserId, guid, cancellationToken);

            return refreshToken != null && refreshToken == dto.RefreshToken;
        }

        public async Task DeactivateToken(CancellationToken cancellationToken)
        {
            await _tokenManager.DeactivateToken(cancellationToken);
        }

        private static IEnumerable<Claim> CreateClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
        }

        private SecurityToken BuildJwt(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentitals = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(_options.Exp);

            return new JwtSecurityToken(
                _options.Issuer,
                _options.Issuer,
                claims,
                expires: expiresAt,
                signingCredentials: credentitals
            );
        }

        private string BuildRefreshToken(User user)
        {
            return _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
        }
    }
}
