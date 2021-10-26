using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArenaApi.DataAccess;
using ArenaApi.Repositories;
using Microsoft.EntityFrameworkCore;
using ArenaApi.Interfaces;
using ArenaApi.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

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
                options.UseNpgsql("Host=localhost;Database=arena;Username=;Password=");
            });
            services.AddControllers();
            services.AddAuthentication(options =>{
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", JwtBearerOptions => {
                JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ciaggggggggggggggggggggggggggggggo")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5),
                };
            });
        }

        public void Configure(IApplicationBuilder app) {
            app.UseMiddleware<ExceptionsMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
