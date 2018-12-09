using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.RentCars.Dto
{
    public class CancelRentCarRequestDto
    {
        [Required]
        public int Id { get; set; }
    }
}
