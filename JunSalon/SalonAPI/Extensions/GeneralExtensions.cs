using System.Linq;
using Microsoft.AspNetCore.Http;

public static class GeneralExtensions
{
    // example of 'this' keyword - method to extend the HttpContext type
    public static string GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User == null)
        {
            return string.Empty;
        }

        return httpContext.User.Claims.Single(x => x.Type == "id").Value;
    }
}