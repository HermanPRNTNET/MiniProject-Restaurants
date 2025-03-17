using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using System.Net;

namespace Restaurants.API.Middlewares
{
    public class ErrorHandlingMiddleware(ILogger <ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFound)
            {
                logger.LogError(notFound.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFound.Message);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("An unexpected fault happened. Please try again later.");
            }
        }
    }
}
