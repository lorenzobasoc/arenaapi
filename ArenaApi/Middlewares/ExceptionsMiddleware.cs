using System;
using System.Threading.Tasks;
using ArenaApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ArenaApi.Middlewares
{
    public class ExceptionsMiddleware : IMiddleware
    {
        private readonly ILogger<Exception> _logger;

        public ExceptionsMiddleware(ILogger<Exception> logger){
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try {
                await next(context);
            } catch (InvalidOperationException ex){
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(ex.Message);
            } catch (NotFoundException ex){
                _logger.LogError(ex, ex.Message); 
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(ex.Message); 
            } catch (Exception ex){
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(_logger);
            }
        }
    }
}