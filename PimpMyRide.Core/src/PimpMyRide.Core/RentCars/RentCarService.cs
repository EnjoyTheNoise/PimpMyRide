using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PimpMyRide.Core.Data.Models;
using PimpMyRide.Core.Data.UnitOfWork;
using PimpMyRide.Core.RentCars.Dto;

namespace PimpMyRide.Core.RentCars
{
    public class RentCarService : IRentCarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentCarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RentCarResponseDto> RentCar(RentCarRequestDto dto, CancellationToken cancellationToken)
        {
            var car = await _unitOfWork.CarRepository.Get(c => c.Id == dto.CarId).SingleOrDefaultAsync(cancellationToken);
            var newRentCar = _mapper.Map<RentCar>(dto);

            if (!car.IsAvailable)
            {
                await _unitOfWork.RentCarRepository.AddAsync(newRentCar, cancellationToken);
                await _unitOfWork.SaveAsync(cancellationToken);
            }
            else
            {
                return null;
            }

            return _mapper.Map<RentCarResponseDto>(newRentCar);

        }

        public async Task<CancelRentCarResponseDto> CancelRentCar(int id, CancellationToken cancellationToken)
        {
            var rentCar = await _unitOfWork.RentCarRepository.Get(r => r.Id == id)
                .SingleOrDefaultAsync(cancellationToken);

            if (rentCar == null)
            {
                return null;
            }

            _unitOfWork.RentCarRepository.Delete(rentCar);
            await _unitOfWork.SaveAsync(cancellationToken);

            var result = _mapper.Map<CancelRentCarResponseDto>(rentCar);

            return result;
        }
    }
}
