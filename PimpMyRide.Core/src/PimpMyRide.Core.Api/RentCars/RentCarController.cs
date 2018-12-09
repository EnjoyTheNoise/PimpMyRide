using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.RentCars;
using PimpMyRide.Core.RentCars.Dto;

namespace PimpMyRide.Core.Api.RentCars
{
    [Route("api/rentcar")]
    [Controller]
    public class RentCarController : Controller
    {
        private readonly IRentCarService _rentCarService;

        public RentCarController(IRentCarService rentCarService)
        {
            _rentCarService = rentCarService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RentCarResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RentCar([FromBody] RentCarRequestDto dto,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _rentCarService.RentCar(dto, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            var response = new ApiOkResponse(result);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CancelRentCarResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelRentCar(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _rentCarService.CancelRentCar(id, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            return NoContent();
        }
    }
}
