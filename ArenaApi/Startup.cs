using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ArenaApi.DataAccess;
using ArenaApi.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ArenaApi.Interfaces;
using ArenaApi.Middlewares;

namespace ArenaApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<Arena>();
            services.AddScoped<ExceptionsMiddleware>();
            services.AddScoped<IFighterRepository, FighterRepository>();
            services.AddDbContext<AppDbContext>(options => {
                options.UseNpgsql("Host=localhost;Database=;Username=;Password=");
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app) {
            /*
            *  exception middleware 
            *   try { 
                 next();
               }catch(NotFOundExc){
                   build 404 response,
               }

               try { 
                 next();
               }catch(NotAuthorizedExc){
                   build 401 response,    //piglia le eccezioni lanciate da [Authorized] sui controller, vedi documentaizone
               }
            *
            */
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
