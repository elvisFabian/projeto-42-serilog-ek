using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace Microsoft.AspNetCore.Http
{
    public class LogConstantes
    {
        public const string user_id = "user_id";
        public const string user_name = "user_name";
        public const string user_email = "user_email";
        public const string body = "{@httpContext_request_body}";
    }

    public class LogUserProperties
    {
        public readonly string UserId;
        public readonly string Email;
        public readonly string Name;

        public LogUserProperties(HttpContext httpContext)
        {
            var user = httpContext.User.Identity as ClaimsIdentity;

            if (user?.IsAuthenticated ?? false)
            {
                UserId = user.FindFirst("sub")?.Value ?? string.Empty;
                Email = user.FindFirst("email")?.Value ?? string.Empty;
                Name = user.FindFirst("name")?.Value ?? string.Empty;
            }
            else
            {
                Name = "anonymous";
            }
        }
    }

    public static class LogContextExtensionsMethods
    {
        public static void SetUserPropertiesOnLogContext(this HttpContext httpContext)
        {
            var user = new LogUserProperties(httpContext);

            LogContext.PushProperty(LogConstantes.user_id, user.UserId);
            LogContext.PushProperty(LogConstantes.user_name, user.Name);
            LogContext.PushProperty(LogConstantes.user_email, user.Email);
        }

        public static void LogBodyInformation<T>(this ILogger logger, T body)
        {
            logger.LogInformation(LogConstantes.body, body);
        }
    }
}