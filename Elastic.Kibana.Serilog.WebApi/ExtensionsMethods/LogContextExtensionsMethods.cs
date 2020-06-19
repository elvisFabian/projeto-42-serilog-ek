using System.Security.Claims;
using Serilog.Context;

namespace Microsoft.AspNetCore.Http
{
    public static class LogContextExtensionsMethods
    {
        public static void SetUserPropertiesOnLogContext(this HttpContext context)
        {
            var user = context.User.Identity as ClaimsIdentity;

            if (user?.IsAuthenticated ?? false)
            {
                var userId = user.FindFirst("sub")?.Value ?? string.Empty;
                var email = user.FindFirst("email")?.Value ?? string.Empty;
                var name = user.FindFirst("name")?.Value ?? string.Empty;

                LogContext.PushProperty("user_id", userId);
                LogContext.PushProperty("user_name", name);
                LogContext.PushProperty("user_email", email);
            }
            else
            {
                LogContext.PushProperty("user_name", "anonymous");
            }
        }
    }
}