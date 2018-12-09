using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.RentCars.Dto;

namespace PimpMyRide.Core.RentCars
{
    public interface IRentCarService
    {
        Task<RentCarResponseDto> RentCar(RentCarRequestDto dto, int id, CancellationToken cancellationToken);
        Task<CancelRentCarResponseDto> CancelRentCar(int id, CancellationToken cancellationToken);
    }
}
