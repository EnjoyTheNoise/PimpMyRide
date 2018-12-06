using System.ComponentModel.DataAnnotations;

namespace PimpMyRide.Core.Api.Tokens.Dto
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
