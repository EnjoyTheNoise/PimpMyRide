namespace PimpMyRide.Core.Api.Tokens.Dto
{
    public class RevokeTokenRequestDto
    {
        public string RefreshToken { get; set; }

        public int UserId { get; set; }
    }
}
