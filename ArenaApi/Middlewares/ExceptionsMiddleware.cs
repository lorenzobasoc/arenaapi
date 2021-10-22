using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ArenaApi.Middlewares
{
    public class ExceptionsMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try {
                await next(context);
            } catch (InvalidOperationException){
                context.Response.StatusCode = 400;
            } catch (KeyNotFoundException){
                context.Response.StatusCode = 400;
            } catch (NullReferenceException){
                context.Response.StatusCode = 404;
            }
        }
    }
}