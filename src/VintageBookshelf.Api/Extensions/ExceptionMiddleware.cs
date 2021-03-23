using System;
using System.Net;
using System.Threading.Tasks;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace VintageBookshelf.Api.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                HandleException(context, e);
            }
        }

        private static void HandleException(HttpContext context, Exception e)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            e.Ship(context);
        }
    }
}