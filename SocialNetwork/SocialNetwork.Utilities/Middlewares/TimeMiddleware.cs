using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace SocialNetwork.Utilities.Middlewares
{
    public class ElapsedTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ElapsedTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            await _next(context);

            sw.Stop();
            context.Response.Headers.Add("X-Time-Elapsed", (sw.ElapsedMilliseconds/100.0).ToString());
        }
    }

    public static class ElapsedTimeMiddlewareExtensions
    {
        public static void ConfigureElapsedTimeMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ElapsedTimeMiddleware>();
        }
    }
}
