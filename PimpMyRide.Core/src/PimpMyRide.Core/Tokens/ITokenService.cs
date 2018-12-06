using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Tokens.Dto;

namespace PimpMyRide.Core.Tokens
{
    public interface ITokenService
    {
        Task<Jwt> CreateToken(TokenCreateDto dto, CancellationToken cancellationToken);
        Task<Jwt> RefreshSecurityToken(TokenDto dto, CancellationToken cancellationToken);
        Task RevokeToken(TokenDto dto, CancellationToken cancellationToken);
        Task DeactivateToken(CancellationToken cancellationToken);
        Task<bool> CheckToken(TokenDto dto, CancellationToken cancellationToken);
    }
}
