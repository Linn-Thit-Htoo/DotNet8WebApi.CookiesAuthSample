using System.Security.Claims;
using DotNet8WebApi.CookiesAuthSample.Db;
using DotNet8WebApi.CookiesAuthSample.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet8WebApi.CookiesAuthSample.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet, Route("/api/account")]
        [Authorize]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                var lst = await _context.Tbl_Users.Where(x => x.IsActive).ToListAsync();
                return Content(lst);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("/api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel requestModel)
        {
            try
            {
                var item = await _context.Tbl_Users.FirstOrDefaultAsync(x =>
                    x.Email == requestModel.Email
                    && x.Password == requestModel.Password
                    && x.IsActive
                );
                if (item is null)
                {
                    return NotFound("No Data Found.");
                }

                var claims = new List<Claim>()
                {
                    new("UserName", item.UserName),
                    new("Email", item.Email),
                    new("UserRole", item.UserRole)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );
                var authProperties = new AuthenticationProperties { };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return Content(item);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost("/api/account/logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Content("Success.");
        }
    }
}
