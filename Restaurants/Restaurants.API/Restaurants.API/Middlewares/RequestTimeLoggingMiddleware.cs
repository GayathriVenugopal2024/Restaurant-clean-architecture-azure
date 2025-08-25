
using System.Diagnostics;

namespace Restaurants.API.Middlewares
{
    public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware>logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await next.Invoke(context);
            sw.Stop();
            if(sw.ElapsedMilliseconds/1000 > 4) {
                logger.LogInformation("Request [{Verb}] at {Path} took {Time} ms", context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds);
                    }
        }
    }
}
