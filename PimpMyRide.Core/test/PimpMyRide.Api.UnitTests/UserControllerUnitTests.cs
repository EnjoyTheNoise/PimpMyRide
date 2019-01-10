using System;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.Api.Infrastructure.Consts;
using PimpMyRide.Core.Api.Users;
using PimpMyRide.Core.Tokens;
using PimpMyRide.Core.Tokens.Dto;
using PimpMyRide.Core.Users;
using PimpMyRide.Core.Users.Dto;
using Xunit;

namespace PimpMyRide.Api.UnitTests
{
    public class UserControllerUnitTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ITokenService> _tokenService;
        private readonly Mock<IMapper> _mapper;
        private readonly UserController _controller;

        private const string ConstPassword = "pedro123";
        private const string ConstEmail = "pedro123@gmail.com";

        public UserControllerUnitTests()
        {
            //Arrange
            _autoMocker = new AutoMocker();
            _userService = _autoMocker.GetMock<IUserService>();
            _tokenService = _autoMocker.GetMock<ITokenService>();
            _mapper = _autoMocker.GetMock<IMapper>();
            _controller = _autoMocker.CreateInstance<UserController>();

            _mapper.Setup(x => x.Map<TokenCreateDto>(It.IsAny<UserLoginRequestDto>())).Returns(
                new TokenCreateDto
                {
                    Email = ConstEmail,
                    Password = ConstPassword
                });

            _userService.Setup(x => x.CredentialsValid(It.IsAny<UserLoginRequestDto>())).Returns(true);
            _userService.Setup(x => x.CredentialsValid(null)).Returns(false);

            _userService.Setup(x => x.UserExist(It.IsAny<string>())).Returns(false);
            _userService.Setup(x => x.UserExist(string.Empty)).Returns(true);

            _userService.Setup(x => x.CreateUserAsync(It.IsAny<UserRegisterRequestDto>(), CancellationToken.None)).ReturnsAsync(
                new UserRegisterResponseDto
                {
                    Id = 1
                });

            _tokenService.Setup(x => x.CreateToken(It.IsAny<TokenCreateDto>(), CancellationToken.None)).ReturnsAsync(
                new Jwt
                {
                    RefreshToken = "refresh",
                    ExpiryDate = new DateTime(2018, 1, 1),
                    SecurityToken = "security"
                });
        }

        [Fact]
        public async void GivenValidDto_WhenRequestingLogin_ExpectOkResponse()
        {
            //Act
            var result = await _controller.Login(new UserLoginRequestDto {Email = ConstEmail, Password = ConstPassword},
                CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<ApiOkResponse>(okResult?.Value);

            var apiOkResponse = okResult.Value as ApiOkResponse;
            Assert.IsType<Jwt>(apiOkResponse?.Result);

            var tokenDto = apiOkResponse.Result as Jwt;
            Assert.NotNull(tokenDto?.RefreshToken);
            Assert.Equal("refresh", tokenDto.RefreshToken);
            Assert.NotNull(tokenDto.SecurityToken);
            Assert.Equal("security", tokenDto.SecurityToken);
            Assert.Equal(new DateTime(2018, 1, 1), tokenDto.ExpiryDate);
        }

        [Fact]
        public async void GivenInvalidCredentials_WhenRequestingLogin_ExpectBadRequest()
        {
            //Act
            var result = await _controller.Login(null, CancellationToken.None);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestRsult = result as BadRequestObjectResult;
            Assert.IsType<ApiResponse>(badRequestRsult?.Value);

            var apiResponse = badRequestRsult.Value as ApiResponse;
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse.Errors.Count > 0);
            Assert.True(apiResponse.Errors.ContainsKey(ErrorMessages.DefaultKey));
            Assert.Equal(ErrorMessages.WrongCredentials, apiResponse.Errors[ErrorMessages.DefaultKey]);
        }

        [Fact]
        public async void GivenValidRequest_WhenRequestingLogout_ExpectNoContent()
        {
            //Act
            var result = await _controller.Logout(CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void GivenUniqueUserData_WhenRequestingRegister_ExpectOkResponse()
        {
            //Act
            var result = await _controller.Register(
                new UserRegisterRequestDto
                {
                    Email = ConstEmail,
                    Password = ConstPassword,
                    ConfirmPassword = ConstPassword
                }, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<ApiOkResponse>(okResult?.Value);

            var apiOkResponse = okResult.Value as ApiOkResponse;
            Assert.IsType<UserRegisterResponseDto>(apiOkResponse?.Result);

            var responseDto = apiOkResponse.Result as UserRegisterResponseDto;
            Assert.NotNull(responseDto?.Id);
            Assert.Equal(1, responseDto.Id);
        }

        [Fact]
        public async void GivenExistingUserData_WhenRequestingRegister_ExpectBadRequest()
        {
            //Act
            var result = await _controller.Register(
                new UserRegisterRequestDto
                {
                    Email = string.Empty,
                    Password = ConstPassword,
                    ConfirmPassword = ConstPassword
                }, CancellationToken.None);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestRsult = result as BadRequestObjectResult;
            Assert.IsType<ApiResponse>(badRequestRsult?.Value);

            var apiResponse = badRequestRsult.Value as ApiResponse;
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse.Errors.Count > 0);
            Assert.True(apiResponse.Errors.ContainsKey(ErrorMessages.EmailKey));
            Assert.Equal(ErrorMessages.EmailExists, apiResponse.Errors[ErrorMessages.EmailKey]);
        }
    }
}
