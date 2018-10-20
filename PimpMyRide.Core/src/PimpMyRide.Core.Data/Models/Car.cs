using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PimpMyRide.Core.Data.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ModelName { get; set; }

        [Required]
        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }

        [Required]
        [ForeignKey("EngineType")]
        public int EngineTypeId { get; set; }

        public EngineType EngineType { get; set; }

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

        public DateTime RentedFrom { get; set; }

        public DateTime RentedTo { get; set; }

        public bool IsAvailable { get; set; }

        public string ImageFileName { get; set; }
    }
}
