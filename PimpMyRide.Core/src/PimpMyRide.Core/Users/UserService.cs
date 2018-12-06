using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.Users.Dto;

namespace PimpMyRide.Core.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserRegisterResponseDto> CreateUserAsync(UserRegisterRequestDto userRegisterRequestDto, CancellationToken cancellationToken)
        {
            GeneratePasswordHashAndSalt(userRegisterRequestDto.Password, out var passwordHash, out var passwordSalt);

            var user = _mapper.Map<User>(userRegisterRequestDto);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            var accountIdDto = new UserRegisterResponseDto { Id = user.Id };

            return accountIdDto;
        }

        private void GeneratePasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool UserExist(string email)
        {
            return _unitOfWork.UserRepository.Get(u => string.Equals(u.Email, email, StringComparison.CurrentCultureIgnoreCase)).Any();
        }

        public User GetUser(string email)
        {
            return _unitOfWork.UserRepository
                .Get(u => string.Equals(u.Email, email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
        }

        public bool CredentialsValid(UserLoginRequestDto userLoginRequestDto)
        {
            var user = GetUser(userLoginRequestDto.Email);
            return user != null && VerifyPassword(user, userLoginRequestDto.Password);
        }

        public bool VerifyPassword(User user, string password)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < password.Length; i++)
                {
                    if (user.PasswordHash[i] != computedHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
