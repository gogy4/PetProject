using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace PetProject.Controllers;

public class CookieUtils
{
    public async Task CreateCookie(string id, HttpContext httpContext)
    { var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}