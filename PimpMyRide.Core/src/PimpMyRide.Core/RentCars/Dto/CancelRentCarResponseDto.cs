using System;

namespace PimpMyRide.Core.RentCars.Dto
{
    public class CancelRentCarResponseDto
    {
        public string User { get; set; }
        public string Car { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
    }
}
