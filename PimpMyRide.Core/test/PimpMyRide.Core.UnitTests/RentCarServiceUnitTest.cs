using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using AutoMapper;
using Moq;
using Moq.AutoMock;
using PimpMyRide.Core.Cars;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.RentCars;
using PimpMyRide.Core.RentCars.Dto;
using Xunit;

namespace PimpMyRide.Core.UnitTests
{
    public class RentCarServiceUnitTest
    {
        private AutoMocker _autoMocker;
        private Mock<IMapper> _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private readonly CarService _carService;
        private readonly RentCarService _rentCarService;

        public RentCarServiceUnitTest()
        {
            //Arrange
            _autoMocker = new AutoMocker();
            _carService = _autoMocker.CreateInstance<CarService>();
            _rentCarService = _autoMocker.CreateInstance<RentCarService>();
            _mapper = _autoMocker.GetMock<IMapper>();
            _unitOfWork = _autoMocker.GetMock<IUnitOfWork>();

            var carList = new List<Car>
            {
                new Car
                {
                    Id = 1,
                    ModelName = "passat",
                    Manufacturer = new Manufacturer
                    {
                        Id = 1,
                        Name = "Volkswagen"
                    },
                    EngineType = new EngineType
                    {
                        Id = 1,
                        Type = "Diesel"
                    },
                    EngineCapacity = 2.0,
                    PriceForDay = 1500,
                    PriceForHour = 200,
                    Mileage = 99999,
                    Collateral = 500,
                    IsAvailable = true
                },
                new Car
                {
                    Id = 2,
                    ModelName = "passat2",
                    Manufacturer = new Manufacturer
                    {
                        Id = 1,
                        Name = "Volkswagen"
                    },
                    EngineType = new EngineType
                    {
                        Id = 1,
                        Type = "Diesel"
                    },
                    EngineCapacity = 2.0,
                    PriceForDay = 1500,
                    PriceForHour = 200,
                    Mileage = 99999,
                    Collateral = 500,
                    IsAvailable = false
                }

            };

            var manufacturerList = new List<Manufacturer>
            {
                new Manufacturer
                {
                    Id = 1,
                    Name = "Volkswagen"
                }
            };

            var engineTypes = new List<EngineType>
            {
                new EngineType
                {
                    Id = 1,
                    Type = "Diesel"
                }
            };

            var userList = new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "admin@elo.com"
                }
            };

            var bookingsList = new List<RentCar>
            {
                new RentCar
                {
                    Id = 1,
                    CarId = 1,
                    UserId = 1
                }
            };

            _unitOfWork.Setup(x => x.CarRepository.Get(It.IsNotNull<Expression<Func<Car, bool>>>()))
                .Returns(
                    new Func<Expression<Func<Car, bool>>, IQueryable<Car>>(
                        x => carList.Where(x.Compile()).AsQueryable())
                    );

            _unitOfWork.Setup(x => x.CarRepository.GetAll()).Returns(carList.AsQueryable());

            _unitOfWork.Setup(x => x.ManufacturerRepository.Get(It.IsNotNull<Expression<Func<Manufacturer, bool>>>()))
                .Returns(
                    new Func<Expression<Func<Manufacturer, bool>>, IQueryable<Manufacturer>>(
                        x => manufacturerList.Where(x.Compile()).AsQueryable())
                    );

            _unitOfWork.Setup(x => x.EngineTypeRepository.Get(It.IsNotNull<Expression<Func<EngineType, bool>>>()))
                .Returns(
                    new Func<Expression<Func<EngineType, bool>>, IQueryable<EngineType>>(
                        x => engineTypes.Where(x.Compile()).AsQueryable())
                    );

            _unitOfWork.Setup(x => x.UserRepository.Get(It.IsNotNull<Expression<Func<User, bool>>>()))
                .Returns(
                    new Func<Expression<Func<User, bool>>, IQueryable<User>>(
                        x => userList.Where(x.Compile()).AsQueryable())
                );

            _unitOfWork.Setup(x => x.RentCarRepository.Get(It.IsNotNull<Expression<Func<RentCar, bool>>>()))
                .Returns(
                    new Func<Expression<Func<RentCar, bool>>, IQueryable<RentCar>>(
                        x => bookingsList.Where(x.Compile()).AsQueryable())
                );

            _mapper.Setup(x => x.Map<RentCar>(It.IsAny<RentCarRequestDto>())).Returns(
                new RentCar
                {
                    CarId = 1,
                    DateStart = new DateTime(2019, 1, 1),
                    DateEnd = new DateTime(2019, 1, 2),
                    UserId = 1
                });

            _mapper.Setup(x => x.Map<RentCarResponseDto>(It.IsAny<RentCar>())).Returns(
                new RentCarResponseDto
                {
                    Id = 1
                });

            _mapper.Setup(x => x.Map<CancelRentCarResponseDto>(It.IsAny<RentCar>())).Returns(
                new CancelRentCarResponseDto
                {
                    Car = "testcar",
                    DateStart = new DateTime(2019, 1, 1),
                    DateEnd = new DateTime(2019, 1, 2),
                    User = "test"
                });
        }

        [Fact]
        public async void GivenValidDto_WhenRentCar_ThenExpectNotNull()
        {
            //Act
            var result = await _rentCarService.RentCar(new RentCarRequestDto
            {
                CarId = 1,
                DateStart = new DateTime(2019, 1, 1),
                DateEnd = new DateTime(2019, 1, 2),
                UserId = 1
            }, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async void GivenInvalidDto_WhenRentCar_ThenExpectNull()
        {
            //Act
            var result = await _rentCarService.RentCar(new RentCarRequestDto
            {
                CarId = 5,
                UserId = 10
            }, CancellationToken.None);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void GivenExistingId_WhenCancelRentCar_ThenExpectNotNull()
        {
            //Act
            var result = await _rentCarService.CancelRentCar(1, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("testcar", result.Car);
            Assert.Equal("test", result.User);
            Assert.Equal(new DateTime(2019, 1, 1), result.DateStart);
            Assert.Equal(new DateTime(2019, 1, 2), result.DateEnd);
        }

        [Fact]
        public async void GivenWrongId_WhenCancelRentCar_ThenExpectNull()
        {
            //Act
            var result = await _rentCarService.CancelRentCar(999, new CancellationToken());

            //Assert
            Assert.Null(result);
        }
    }
}
