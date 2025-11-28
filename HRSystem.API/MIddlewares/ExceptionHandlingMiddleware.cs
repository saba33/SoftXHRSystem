using HRSystem.Application.DTOs.Common;
using Serilog;
using System.Net;

namespace HRSystem.API.MIddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid().ToString();

                Log.ForContext("ErrorId", errorId)
                   .ForContext("Path", context.Request.Path)
                   .ForContext("Query", context.Request.QueryString.Value)
                   .ForContext("Method", context.Request.Method)
                   .ForContext("IP", context.Connection.RemoteIpAddress?.ToString())
                   .Error(ex, "Unhandled exception occurred.");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Internal server error occurred.",
                    errorId = errorId,
                    path = context.Request.Path,
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
