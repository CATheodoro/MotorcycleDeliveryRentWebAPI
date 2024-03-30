using System.Data;
using System.Net;
using System.Text.Json;

namespace MotorcycleDeliveryRentWebAPI.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode statusCode;
            string stackTrace = String.Empty;
            string message;
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(DBConcurrencyException))
            {
                message = ex.Message;
                statusCode = HttpStatusCode.BadRequest;
                stackTrace = ex.StackTrace;
            }
            else
            {
                message = ex.Message;
                statusCode = HttpStatusCode.InternalServerError;
                stackTrace = ex.StackTrace;
            }

            var response = JsonSerializer.Serialize(new { statusCode, message, stackTrace });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }
}