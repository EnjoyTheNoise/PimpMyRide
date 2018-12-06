using System;

namespace PimpMyRide.Core.Tokens.Dto
{
    public class Jwt
    {
        public string SecurityToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
