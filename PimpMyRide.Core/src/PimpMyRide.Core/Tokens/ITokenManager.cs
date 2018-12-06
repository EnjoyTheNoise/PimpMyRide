using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Infrastructure.Enums;

namespace PimpMyRide.Core.Tokens
{
    public interface ITokenManager
    {
        Task<TokenState> IsTokenActive(CancellationToken cancellationToken);
        Task DeactivateToken(CancellationToken cancellationToken);
        Task<TokenState> IsActive(string token, CancellationToken cancellationToken);
        Task Deactivate(string token, CancellationToken cancellationToken);
        Task<string> GetRefreshToken(int userId, string guid, CancellationToken cancellationToken);
        Task<string> GetSecurityToken(int userId, string guid, CancellationToken cancellationToken);
        Task<string> GetGuidFromRequest(CancellationToken cancellationToken);
        Task AddNewTokens(string newToken, string refreshToken, int userId, string guid, CancellationToken cancellationToken);
        Task SetNewSecurityToken(string newToken, int userId, string guid, CancellationToken cancellationToken);
        Task RevokeTokens(int userId, string guid, CancellationToken cancellationToken);
    }
}
