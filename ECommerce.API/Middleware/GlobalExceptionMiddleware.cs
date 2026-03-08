using ECommerce.Application.Common.Exceptions;
using ECommerce.Domain.Exceptions;
using System.Text.Json;

namespace ECommerce.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            { 
                NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
                ValidationException ex => (StatusCodes.Status400BadRequest, ex.Message),
                ConflictException ex => (StatusCodes.Status409Conflict, ex.Message),
                UnauthorizedException ex => (StatusCodes.Status401Unauthorized, ex.Message),
                DomainExceptions ex => (StatusCodes.Status400BadRequest, ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = JsonSerializer.Serialize( new 
            {
                statusCode = (int)statusCode,
                message
            });

            return context.Response.WriteAsync(response);
        }
    }
}
