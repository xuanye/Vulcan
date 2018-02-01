using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UUAC.Common;

namespace UUAC.WebApp.Libs
{
    public class MockAppUserMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly MockAppUserOption _option;

        public MockAppUserMiddleware(RequestDelegate next, MockAppUserOption option = null)
        {
            _next = next;
            _option = option ?? new MockAppUserOption() { MockUserId = "admin" };
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                string userId = _option.MockUserId;


                var claims = new[] { new Claim(ClaimTypes.Name, userId) };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,   new ClaimsPrincipal(identity));
                
            }

            await _next.Invoke(context);
        }
    }

    public class MockAppUserOption
    {
        public string MockUserId { get; set; }
    }

    public static class MockAppUserExtensions
    {
        public static IApplicationBuilder UseMockAppUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MockAppUserMiddleware>();
        }

        public static IApplicationBuilder UseMockAppUser(this IApplicationBuilder builder, MockAppUserOption option)
        {
            return builder.UseMiddleware<MockAppUserMiddleware>(option);
        }
    }
}
