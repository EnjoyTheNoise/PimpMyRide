using System;
using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.RentCars.Dto
{
    public class RentCarRequestDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int CarId { get; set; }
        [Required]
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
