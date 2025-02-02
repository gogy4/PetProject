using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace PetProject.Controllers;

public class CookieUtils
{
    public async Task CreateCookie(string id, HttpContext httpContext)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }

    public async Task DeleteCookie(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        foreach (var cookie in httpContext.Request.Cookies.Keys) httpContext.Response.Cookies.Delete(cookie);

        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
    }
}