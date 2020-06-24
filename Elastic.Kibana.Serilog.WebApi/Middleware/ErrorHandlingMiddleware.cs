using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using Serilog.Context;

namespace Elastic.Kibana.Serilog.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                httpContext.SetUserPropertiesOnLogContext();
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            logger.LogCritical(ex, "Erro não tratado, capturado pelo Middleware: {Middleware}", nameof(ErrorHandlingMiddleware));

            var code = HttpStatusCode.InternalServerError;

            // if      (ex is MyNotFoundException)     code = HttpStatusCode.NotFound;
            // else if (ex is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            // else if (ex is MyException)             code = HttpStatusCode.BadRequest;

            var errorDetail = ex.Demystify().ToString();

            var result = JsonConvert.SerializeObject(new
            {
                Title = "An unexpected error occurred!",
                Detail = errorDetail,
            });

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int) code;
            await httpContext.Response.WriteAsync(result);
        }
    }
}