using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.Cars.Dto
{
    public class AddCarRequestDto
    {
        [Required]
        public string ModelName { get; set; }

        [Required]
        public int ManufacturerId { get; set; }

        [Required]
        public int EngineTypeId { get; set; }

        [Required]
        public double EngineCapacity { get; set; }

        [Required]
        public double PriceForDay { get; set; }

        [Required]
        public double PriceForHour { get; set; }

        [Required]
        public double Mileage { get; set; }

        [Required]
        public double Collateral { get; set; }
    }
}
