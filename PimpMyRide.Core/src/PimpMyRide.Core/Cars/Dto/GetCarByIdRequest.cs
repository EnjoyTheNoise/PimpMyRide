using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.Cars.Dto
{
    public class GetCarByIdRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
