using Serilog;
using System.Diagnostics;

namespace Backend.Api.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            var requestInfo = $"{request.Method} {request.Path}";

            Log.Information("Incoming Request: {RequestInfo}", requestInfo, context.Connection.RemoteIpAddress);

            await _next(context);

            stopwatch.Stop();

            var response = context.Response;
            Log.Information("Outgoing Response: {StatusCode} for {RequestInfo} in {ElapsedMilliseconds}ms",
                response.StatusCode, requestInfo, stopwatch.ElapsedMilliseconds);
        }
    }
}
