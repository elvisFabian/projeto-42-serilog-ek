namespace Microsoft.AspNetCore.Mvc
{
    public static class IActionResultExtensionsMethods
    {
        public static IActionResult AsHttpResponse<T>(this T result)
        {
            if (result == null) return new NotFoundResult();

            return new OkObjectResult(result);
        }
    }
}