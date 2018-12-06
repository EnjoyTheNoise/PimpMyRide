using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PimpMyRide.Core.Infrastructure.Enums;
using PimpMyRide.Core.Tokens;

namespace PimpMyRide.Core.Api.Infrastructure.Middleware
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly ITokenManager _tokenManager;
        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsTokenActive(default(CancellationToken)) == TokenState.TokenIsActive)
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
