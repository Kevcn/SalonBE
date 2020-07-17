using AutoMapper;
using Financial_Market_API.Authorization;
using Financial_Market_API.Configuration;
using Financial_Market_API.Filters;
using Financial_Market_API.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Financial_Market_API.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration config)
        {
            // Replaced by AddControllers in Core 3.0
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            // New to .NET Core 3.0, includes services required for API only - Authorization, Validation, formatters CORS... excluded Razor pages or view rending
            services.AddControllers(options => options.Filters.Add<ValidationFilter>())
                // RegisterValidatorsFromAssemblyContaining -> This only works with the validator class derived from AbstractValidator
                .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            // ******************* JWT setup *******************
            // ******************* JWT setup *******************
            // ******************* JWT setup *******************

            var jwtSettings = new JwtSettings();
            config.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            // Required to be accessed anywhere within the application
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(options =>
                {
                    // To use JWT as Authentication method
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Declare what the JWT token will look like
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            
            // ******************* Identity setup *******************
            // ******************* Identity setup *******************
            // ******************* Identity setup *******************
            services.AddScoped<IIdentityService, IdentityService>();


            // ******************* For Claims and Roles Authorization *******************
            // ******************* For Claims and Roles Authorization *******************
            // ******************* For Claims and Roles Authorization *******************
            services.AddAuthorization(options => 
            {
                // Claim example 
                options.AddPolicy("AccessToStocks", policy => policy.RequireClaim("GetStock", "true"));

                // Custom policy
                options.AddPolicy("GmailUser", policy => policy.AddRequirements(new UserOfGmail("gmail.com")));
            });

            // Custom policy
            services.AddSingleton<IAuthorizationHandler, GmailHandler>();


            // ******************* AutoMapper for response/domain/DTO objects*******************
            // ******************* AutoMapper for response/domain/DTO objects*******************
            // ******************* AutoMapper for response/domain/DTO objects*******************
            services.AddAutoMapper(typeof(Startup));
        }
    }
}
