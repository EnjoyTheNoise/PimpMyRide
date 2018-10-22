using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PimpMyRide.Core.Cars.Dto;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;

namespace PimpMyRide.Core.Cars
{
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddCarResponseDto> AddCarAsync(AddCarRequestDto dto, CancellationToken cancellationToken)
        {
            var newCar = _mapper.Map<Car>(dto);
            newCar.IsAvailable = true;

            await _unitOfWork.CarRepository.AddAsync(newCar, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<AddCarResponseDto>(newCar);
        }

        public async Task<GetCarByIdResponse> GetCarById(int id, CancellationToken cancellationToken)
        {
            var car = await _unitOfWork.CarRepository.Get(c => c.Id == id).SingleOrDefaultAsync(cancellationToken);
            var manufacturer = await _unitOfWork.ManufacturerRepository.Get(m => m.Id == car.ManufacturerId)
                .SingleOrDefaultAsync(cancellationToken);
            var engine = await _unitOfWork.EngineTypeRepository.Get(e => e.Id == car.EngineTypeId)
                .SingleOrDefaultAsync(cancellationToken);

            var result = _mapper.Map<GetCarByIdResponse>(car);
            result.Manufacturer = manufacturer.Name;
            result.EngineType = engine.Type;

            return result;
        }

        public async Task<IEnumerable<GetAllCarsResponseDto>> GetAll(CancellationToken cancellationToken)
        {
            var cars = await _unitOfWork.CarRepository.Get(c => c.IsAvailable).Include(c => c.Manufacturer)
                .Include(c => c.EngineType).ToListAsync(cancellationToken);

            var result = new List<GetAllCarsResponseDto>();
            foreach (var car in cars)
            {
                var carDto = _mapper.Map<GetAllCarsResponseDto>(car);
                carDto.Manufacturer = car.Manufacturer.Name;
                carDto.EngineType = car.EngineType.Type;
                result.Add(carDto);
            }

            return result;
        }
    }
}
