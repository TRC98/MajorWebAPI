using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Core.DataAccess;
using WebAPI.Core.Service;
using WebAPI.DataAccess;

namespace MajorWebAPI.Extensions
{
    public static class ConfigureService
    {
        private static IConfiguration _Configuration;

        public static void Configrue(IServiceCollection services,IConfiguration configuration)
        {
            _Configuration = configuration;

            services.AddCors();
            services.InjectDependancies();
            services.AddJWTAuthConfigurations();
            services.AddControllers();

        }
        public static IServiceCollection InjectDependancies(this IServiceCollection services)
        {
            services.AddScoped<IDataAccessService, DataAccessService>(ctx => new DataAccessService(_Configuration.GetConnectionString("SqlConnectionString")));
            services.AddTransient<IAuthService, AuthService>();

            return services;
        }
        public static IServiceCollection AddJWTAuthConfigurations(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _Configuration["JWT:Issuer"],
                    ValidAudience = _Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}
