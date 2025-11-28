using Serilog;
using System.Diagnostics;

namespace HRSystem.API.MIddlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var request = context.Request;
            var path = request.Path;
            var method = request.Method;
            var ip = context.Connection.RemoteIpAddress?.ToString();

            await _next(context);

            stopwatch.Stop();

            Log.Information(
                "{Method} {Path} completed in {Elapsed}ms from {IP}",
                method, path, stopwatch.ElapsedMilliseconds, ip
            );
        }
    }
}
