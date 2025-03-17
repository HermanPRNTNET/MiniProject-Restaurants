using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Restaurants.API.Middlewares;
public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        #region MyRegion
        //var watch = new Stopwatch();
        //watch.Start();
        //await next.Invoke(context);
        //watch.Stop();
        //var elapsed = watch.ElapsedMilliseconds;
        //if (elapsed > 500)
        //{
        //    var path = context.Request.Path;
        //    var method = context.Request.Method;
        //    var queryString = context.Request.QueryString;
        //    var statusCode = context.Response.StatusCode;
        //    var message = $"Request {method} {path}{queryString} responded with {statusCode} in {elapsed} ms";
        //    Console.WriteLine(message);
        //}

        #endregion


        var stopWatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopWatch.Stop();

        if (stopWatch.ElapsedMilliseconds / 1000 > 4)
        {
            logger.LogInformation("Request [{Verb}] at {Path} took {Time} Ms"
                ,context.Request.Method
                ,context.Request.Path
                ,stopWatch.ElapsedMilliseconds);
        }
    }
}
