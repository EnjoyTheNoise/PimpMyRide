using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;
using PimpMyRide.Core.Infrastructure.Enums;

namespace PimpMyRide.Core.Tokens
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContext;
        private const string RefreshTokenFormat = "$refresh_token_{0}_{1}";
        private const string AccessTokenFormat = "$access_token_{0}_{1}";

        public TokenManager(IDistributedCache cache, IHttpContextAccessor contextAccessor)
        {
            _cache = cache;
            _httpContext = contextAccessor;
        }

        public async Task<TokenState> IsTokenActive(CancellationToken cancellationToken)
        {
            return await IsActive(await GetTokenFromRequest(cancellationToken), cancellationToken);
        }

        public async Task DeactivateToken(CancellationToken cancellationToken)
        {
            await Deactivate(await GetTokenFromRequest(cancellationToken), cancellationToken);
        }

        public async Task<TokenState> IsActive(string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return TokenState.TokenIsActive;
            }

            if (!(new JwtSecurityTokenHandler().ReadToken(token) is JwtSecurityToken jwt))
            {
                return TokenState.TokenIsNotActive;
            }

            var userId = jwt.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var guid = await GetGuidFromRequest(cancellationToken);
            var cachedToken = await GetSecurityToken(int.Parse(userId), guid, cancellationToken);

            return cachedToken == token ? TokenState.TokenIsActive : TokenState.TokenIsNotActive;
        }

        public async Task Deactivate(string token, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            if (!(new JwtSecurityTokenHandler().ReadToken(token) is JwtSecurityToken jwt))
            {
                return;
            }

            var userId = jwt.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var guid = await GetGuidFromRequest(cancellationToken);

            await RevokeTokens(int.Parse(userId), guid, cancellationToken);
        }

        public async Task<string> GetRefreshToken(int userId, string guid, CancellationToken cancellationToken)
        {
            return await _cache.GetStringAsync(string.Format(RefreshTokenFormat, userId, guid), cancellationToken);
        }

        public async Task<string> GetSecurityToken(int userId, string guid, CancellationToken cancellationToken)
        {
            return await _cache.GetStringAsync(string.Format(AccessTokenFormat, userId, guid), cancellationToken);
        }

        public async Task AddNewTokens(string token, string refreshToken, int userId, string guid, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(string.Format(AccessTokenFormat, userId, guid), token, cancellationToken);
            await _cache.SetStringAsync(string.Format(RefreshTokenFormat, userId, guid), refreshToken, cancellationToken);
        }

        public async Task SetNewSecurityToken(string newToken, int userId, string guid, CancellationToken cancellationToken)
        {
            await _cache.SetStringAsync(string.Format(AccessTokenFormat, userId, guid), newToken, cancellationToken);
        }

        public async Task RevokeTokens(int userId, string guid, CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync(string.Format(AccessTokenFormat, userId, guid), cancellationToken);
            await _cache.RemoveAsync(string.Format(RefreshTokenFormat, userId, guid), cancellationToken);
        }

        public async Task<string> GetGuidFromRequest(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var header = _httpContext.HttpContext.Request.Headers["clientGuid"];
                return header == StringValues.Empty ? string.Empty : header.Single();
            }, cancellationToken);
        }

        private async Task<string> GetTokenFromRequest(CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                var header = _httpContext.HttpContext.Request.Headers["authorization"];
                return header == StringValues.Empty ? string.Empty : header.Single().Split(" ").Last();
            }, cancellationToken);
        }
    }
}
