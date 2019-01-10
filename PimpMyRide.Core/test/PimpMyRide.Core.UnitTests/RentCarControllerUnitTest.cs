using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.Api.RentCars;
using PimpMyRide.Core.Cars;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.RentCars;
using PimpMyRide.Core.RentCars.Dto;
using Xunit;

namespace PimpMyRide.Core.UnitTests
{
    public class RentCarControllerUnitTest
    {
        private AutoMocker _autoMocker;
        private Mock<IRentCarService> _rentCarService;
        private Mock<IMapper> _mapper;
        private readonly RentCarController _controller;

        public RentCarControllerUnitTest()
        {
            _autoMocker = new AutoMocker();
            _rentCarService = _autoMocker.GetMock<IRentCarService>();
            _mapper = _autoMocker.GetMock<IMapper>();
            _controller = _autoMocker.CreateInstance<RentCarController>();

            _rentCarService.Setup(x => x.RentCar(It.IsAny<RentCarRequestDto>(),
                CancellationToken.None)).ReturnsAsync(new RentCarResponseDto {Id = 1});

            _rentCarService.Setup(x => x.CancelRentCar(1, CancellationToken.None)).ReturnsAsync(new CancelRentCarResponseDto
                {
                    User = "lol",
                    Car = "passat",
                    DateStart = new DateTime(2018,1,1),
                    DateEnd = new DateTime(2018,1,3)
                });
        }

        [Fact]
        public async Task RentCarPost_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            // Act
            var result = await _controller.RentCar(null, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsType<ApiResponse>(badRequestResult?.Value);

            var apiResponse = badRequestResult.Value as ApiResponse;
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse.Errors.Count > 0);
        }

        [Fact]
        public async Task RentCarPost_ReturnOkRequestResult_WhenModelStateIsValid()
        {
            // Act
            var result = await _controller.RentCar(new RentCarRequestDto()
            {
                UserId = 1,
                CarId = 1,
                DateStart = new DateTime(2018, 1, 1),
                DateEnd = new DateTime(2018, 1, 3)
            }, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<ApiOkResponse>(okResult?.Value);
        }

        [Fact]
        public async Task CancelRentCarDelete_ReturnBadRequestResult_WhenModelStateIsInvalid()
        {
            //Act
            var result = await _controller.CancelRentCar(1, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CancelRentCarDelete_ReturnNoContent_WhenModelStateIsValid()
        {
            // Act
            var result = await _controller.CancelRentCar(5, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsType<ApiResponse>(badRequestResult?.Value);

            var apiResponse = badRequestResult.Value as ApiResponse;
            Assert.NotNull(apiResponse?.Errors);
            Assert.True(apiResponse.Errors.Count > 0);
        }
    }
}
