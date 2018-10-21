using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using PimpMyRide.Core.Api.Infrastructure.Consts;

namespace PimpMyRide.Core.Api.Infrastructure
{
    public class ApiResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Errors { get; set; }

        public ApiResponse(int statusCode, Dictionary<string, string> errorMessage = null)
        {
            Errors = errorMessage ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public static Dictionary<string, string> CreateErrorMessage(string message, string field = ErrorMessages.DefaultField)
        {
            return new Dictionary<string, string> { { field, message } };
        }

        private static Dictionary<string, string> GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return CreateErrorMessage(ErrorMessages.BadRequest);
                case 404:
                    return CreateErrorMessage(ErrorMessages.NotFound);
                case 500:
                    return CreateErrorMessage(ErrorMessages.InternalError);
                default:
                    return null;
            }
        }
    }

    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result = null) : base(200)
        {
            Result = result;
        }
    }

    public class ApiBadRequestResponse : ApiResponse
    {
        public ApiBadRequestResponse(ModelStateDictionary modelState) : base(400)
        {
            Errors = new Dictionary<string, string>();

            foreach (var errorKey in modelState)
            {
                var errorValues = modelState.Where(e => e.Key == errorKey.Key).SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToString();

                Errors.Add(errorKey.Key, errorValues);
            }
        }
    }
}