using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Users.Dto;

namespace PimpMyRide.Core.Users
{
    public interface IUserService
    {
        Task<UserRegisterResponseDto> CreateUserAsync(UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken);
        bool UserExist(string email);
        bool CredentialsValid(UserLoginRequestDto userLoginRequestDto);
        bool VerifyPassword(User user, string password);
    }
}
