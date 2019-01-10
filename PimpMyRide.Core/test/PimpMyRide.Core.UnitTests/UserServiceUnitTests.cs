using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using AutoMapper;
using Moq;
using Moq.AutoMock;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.Users;
using PimpMyRide.Core.Users.Dto;
using Xunit;

namespace PimpMyRide.Core.UnitTests
{
    public class UserServiceUnitTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly UserService _userService;

        private const string ConstPassword = "pedro123";
        private const string ConstEmail = "pedro123@gmail.com";

        public UserServiceUnitTests()
        {
            //Arrange
            _autoMocker = new AutoMocker();
            _userService = _autoMocker.CreateInstance<UserService>();
            _mapper = _autoMocker.GetMock<IMapper>();
            _unitOfWork = _autoMocker.GetMock<IUnitOfWork>();

            var list = new List<User>
            {
                new User
                {
                    Email = ConstEmail,
                    Id = 1,
                    RoleId = 1,
                    PasswordHash = GeneratePasswordHash(),
                    PasswordSalt = new byte[] {1}
                }
            };

            _mapper.Setup(x => x.Map<User>(It.IsAny<UserRegisterRequestDto>())).Returns(
                new User
                {
                    Email = ConstEmail
                });         

            _unitOfWork.Setup(x => x.UserRepository.Get(It.IsNotNull<Expression<Func<User, bool>>>())).Returns(
                new Func<Expression<Func<User, bool>>, IQueryable<User>>(
                    x => list.Where(x.Compile()).AsQueryable())
                );
        }

        private static byte[] GeneratePasswordHash()
        {
            using (var hmac = new HMACSHA512(new byte[]{ 1 }))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(ConstPassword));
            }
        }

        [Fact]
        public async void GivenValidDto_WhenCreatingUser_ExpectNewId()
        {
            //Act
            var result = await _userService.CreateUserAsync(
                new UserRegisterRequestDto
                {
                    Email = ConstEmail,
                    Password = ConstPassword,
                    ConfirmPassword = ConstPassword
                }, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Id);
        }

        [Fact]
        public void GivenValidEmail_WhenCheckingIfUserExists_ExpectTrue()
        {
            //Act
            var result = _userService.UserExist(ConstEmail);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenInvalidEmail_WhenCheckingIfUserExists_ExpectFalse()
        {
            //Act
            var result = _userService.UserExist("XD");

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenValidEmail_WhenGettingUser_ExpectNotNull()
        {
            //Act
            var result = _userService.GetUser(ConstEmail);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(1, result.RoleId);
            Assert.Equal(ConstEmail, result.Email);
            Assert.Equal(new byte[] { 1 }, result.PasswordSalt);
            Assert.Equal(GeneratePasswordHash(), result.PasswordHash);
        }

        [Fact]
        public void GivenInvalidEmail_WhenGettingUser_ExpectNull()
        {
            //Act
            var result = _userService.GetUser("XD");

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GivenValidCredentials_WhenCheckingCredentials_ExpectTrue()
        {
            //Act
            var result =
                _userService.CredentialsValid(new UserLoginRequestDto {Email = ConstEmail, Password = ConstPassword});

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void GivenInvalidEmail_WhenCheckingCredentials_ExpectFalse()
        {
            //Act
            var result =
                _userService.CredentialsValid(new UserLoginRequestDto { Email = "XD", Password = ConstPassword });

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GivenInvalidPassword_WhenCheckingCredentials_ExpectFalse()
        {
            //Act
            var result =
                _userService.CredentialsValid(new UserLoginRequestDto { Email = ConstEmail, Password = "XD" });

            //Assert
            Assert.False(result);
        }
    }
}
