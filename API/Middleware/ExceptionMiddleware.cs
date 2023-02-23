using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    // this is our Exception Handling Middleware
    public class ExceptionMiddleware
    {
        // IHostEnvironment enables us to check if we are in development mode
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;
        // RequestDelegate helps to move on to next Middleware
        public ExceptionMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger, 
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context )
        {
            try
            {
                 // if there is no Exception, move on to next middleware
                await _next(context); 
            }
            // if there is an Exception, then we wanna catch it
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                // this sets the error code to 500, server error
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                // format data into camelCase which is a json convention format 
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                // conver to json
                var json = JsonSerializer.Serialize(response);

                await  context.Response.WriteAsync(json);
            }
        }
    }
}