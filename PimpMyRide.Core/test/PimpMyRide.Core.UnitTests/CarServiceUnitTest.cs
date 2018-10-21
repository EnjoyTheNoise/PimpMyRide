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
using PimpMyRide.Core.Cars.Dto;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using Shouldly;
using Xunit;

namespace PimpMyRide.Core.UnitTests
{
    public class CarServiceUnitTest
    {
        private AutoMocker _autoMocker;
        private Mock<IMapper> _mapper;
        private Mock<IUnitOfWork> _unitOfWork;
        private readonly CarService _carService;

        public CarServiceUnitTest()
        {
            _autoMocker = new AutoMocker();
            _carService = _autoMocker.CreateInstance<CarService>();
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
                    IsAvailable = true
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
        }

        [Fact]
        public void GivenExistingId_WhenGettingCarById_ThenExpectNotNull()
        {
            Assert.NotNull(_carService.GetCarById(1, new CancellationToken()));
        }

        [Fact]
        public void GivenWrongId_WhenGettingCarById_ThenExpectTaskFaulted()
        {
            var result = _carService.GetCarById(999, new CancellationToken());

            result.Status.ShouldBe(System.Threading.Tasks.TaskStatus.Faulted);
        }

        [Fact]
        public void GivenValidDto_WhenAddingNewCar_ThenExpectNotNull()
        {
            Assert.NotNull(_carService.AddCarAsync(new AddCarRequestDto
            {
                ModelName = "passat",
                ManufacturerId = 1,
                EngineTypeId = 1,
                EngineCapacity = 1.8,
                PriceForDay = 15000,
                PriceForHour = 1500,
                Mileage = 999999,
                Collateral = 50000
            }, new CancellationToken()));
        }

        [Fact]
        public void GivenInvalidDto_WhenAddingNewCar_ThenExpectTaskFaulted()
        {
            var result = _carService.AddCarAsync(new AddCarRequestDto
            {
                ModelName = "XDDDDDDDD"
            }, new CancellationToken());

            result.Status.ShouldBe(System.Threading.Tasks.TaskStatus.Faulted);
        }

        [Fact]
        public void GivenRequest_WhenGettingAllCars_ThenExpectNotNull()
        {
            Assert.NotNull(_carService.GetAll(new CancellationToken()));
        }
    }
}
