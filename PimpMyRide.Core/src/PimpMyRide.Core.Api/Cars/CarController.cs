using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PimpMyRide.Core.Api.Infrastructure;
using PimpMyRide.Core.Cars;
using PimpMyRide.Core.Cars.Dto;

namespace PimpMyRide.Core.Api.Cars
{
    [Route("api/car")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAllCarsResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCars(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _carService.GetAll(cancellationToken);

            if (result == null)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            var response = new ApiOkResponse(result);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetCarByIdResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCarById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _carService.GetCarById(id, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            var response = new ApiOkResponse(result);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddCarResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCarAsync([FromBody] AddCarRequestDto dto, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _carService.AddCarAsync(dto, cancellationToken);

            if (result == null)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
            }

            var response = new ApiOkResponse(result);
            return Ok(response);
        }
    }
}
