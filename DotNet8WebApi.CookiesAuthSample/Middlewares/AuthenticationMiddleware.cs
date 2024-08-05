using Dumpify;

namespace DotNet8WebApi.CookiesAuthSample.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cookieName = context.Request.Cookies["auth"];
        cookieName.Dump("Cookie Name: ");

        await _next(context);
    }
}
