using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PimpMyRide.Core.Cars.Dto;

namespace PimpMyRide.Core.Cars
{
    public interface ICarService
    {
        Task<AddCarResponseDto> AddCarAsync(AddCarRequestDto dto, CancellationToken cancellationToken);
        Task <GetCarByIdResponse> GetCarById(int id, CancellationToken cancellationToken);
        Task<IEnumerable<GetAllCarsResponseDto>> GetAll(CancellationToken cancellationToken);
    }
}
