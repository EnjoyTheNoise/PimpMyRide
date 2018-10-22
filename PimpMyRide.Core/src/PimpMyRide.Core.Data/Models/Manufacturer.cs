using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? NumberOfCars { get; set; }
    }
}
