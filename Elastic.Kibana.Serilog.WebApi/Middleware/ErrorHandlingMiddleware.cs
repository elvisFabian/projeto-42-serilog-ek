using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Elastic.Kibana.Serilog.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                context.SetUserPropertiesOnLogContext();
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Erro não tratado, capturado pelo ErrorHandlingMiddleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            // if      (ex is MyNotFoundException)     code = HttpStatusCode.NotFound;
            // else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            // else if (ex is MyException)             code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new {error = ex.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}