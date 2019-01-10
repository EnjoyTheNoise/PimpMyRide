using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using AutoMapper;
using Moq;
using Moq.AutoMock;
using PimpMyRide.Core.Cars;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.RentCars;
using PimpMyRide.Core.RentCars.Dto;
using Shouldly;
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
            _autoMocker = new AutoMocker();
            _carService = _autoMocker.CreateInstance<CarService>();
            _rentCarService = _autoMocker.CreateInstance<RentCarService>();
            _mapper = _autoMocker.GetMock<IMapper>();
            _unitOfWork = _autoMocker.GetMock<IUnitOfWork>();

            _unitOfWork.Setup(x => x.CarRepository.Get(It.IsNotNull<Expression<Func<Car, bool>>>())).Returns(
                new List<Car>
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
                    }
                }.AsQueryable());

            _unitOfWork.Setup(x => x.CarRepository.GetAll()).Returns(new List<Car>
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

            }.AsQueryable());

            _unitOfWork.Setup(x => x.ManufacturerRepository.Get(It.IsNotNull<Expression<Func<Manufacturer, bool>>>()))
                .Returns(
                    new List<Manufacturer>
                    {
                        new Manufacturer
                        {
                            Id = 1,
                            Name = "Volkswagen"
                        }
                    }.AsQueryable);

            _unitOfWork.Setup(x => x.EngineTypeRepository.Get(It.IsNotNull<Expression<Func<EngineType, bool>>>()))
                .Returns(new List<EngineType>
                {
                    new EngineType
                    {
                        Id = 1,
                        Type = "Diesel"
                    }
                }.AsQueryable);

            _unitOfWork.Setup(x => x.UserRepository.Get(It.IsNotNull<Expression<Func<User, bool>>>()))
                .Returns(new List<User>
                {
                    new User
                    {
                        Id = 1,
                        Email = "admin@elo.com"
                    } 
                }.AsQueryable);
        }

        [Fact]
        public void GivenValidDto_WhenRentCar_ThenExpectNotNull()
        {
            Assert.NotNull(_rentCarService.RentCar(new RentCarRequestDto
            {
                CarId = 1,
                DateStart = DateTime.Now,
                DateEnd = DateTime.Now.AddDays(1),
                UserId = 1
            }, new CancellationToken()));
        }

        [Fact]
        public async void GivenInvalidDto_WhenRentCar_ThenExpectNull()
        {
            var result = await _rentCarService.RentCar(new RentCarRequestDto
            {
                CarId = 5,
                UserId = 10
            }, new CancellationToken());

            Assert.NotNull(result);
        }

        [Fact]
        public void GivenExistingId_WhenCancelRentCar_ThenExpectNotNull()
        {
            Assert.NotNull(_rentCarService.CancelRentCar(1, new CancellationToken()));
        }

        [Fact]
        public void GivenWrongId_WhenCancelRentCar_ThenExpectTaskFaulted()
        {
            var result = _rentCarService.CancelRentCar(999, new CancellationToken());

            result.Status.ShouldBe(System.Threading.Tasks.TaskStatus.Faulted);
        }
    }
}
