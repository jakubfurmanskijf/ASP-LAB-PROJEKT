using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Library.Middleware
{
    public class LastVisitMiddleware
    {
        private readonly RequestDelegate _next;

        public LastVisitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Retrieve the LastVisit cookie
            var lastVisit = context.Request.Cookies["LastVisit"];

            // If the cookie exists, add it to the Response headers for debugging or display
            if (!string.IsNullOrEmpty(lastVisit))
            {
                context.Items["LastVisit"] = lastVisit; // Store for later use (e.g., in controllers)
            }

            // Update the LastVisit cookie with the current time
            context.Response.Cookies.Append(
                "LastVisit",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30) // Cookie expires in 30 days
                }
            );

            // Pass control to the next middleware
            await _next(context);
        }
    }
}