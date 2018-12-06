namespace PimpMyRide.Core.Users.Dto
{
    public class UserRegisterRequestDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
