using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PimpMyRide.Core.Api.Infrastructure.Consts
{
    public static class ErrorMessages
    {
        public const string BadRequest = "Server could not understand the request due to invalid syntax";
        public const string NotFound = "Resource not found";
        public const string InternalError = "An unhandled error occurred";

        public const string WrongCredentials = "Wrong username or password";
        public const string EmailExists = "Email already exists";

        public const string DefaultField = "Message";
        public const string DefaultKey = "Error";
        public const string EmailKey = "Email";
    }
}
