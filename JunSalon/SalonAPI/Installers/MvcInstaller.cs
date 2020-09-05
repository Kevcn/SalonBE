using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SalonAPI.Configuration;

namespace SalonAPI.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration config)
        {
            // Replaced by AddControllers in Core 3.0
            //services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            // New to .NET Core 3.0, includes services required for API only - Authorization, Validation, formatters CORS... excluded Razor pages or view rending
            services.AddControllers()
                // RegisterValidatorsFromAssemblyContaining -> This only works with the validator class derived from AbstractValidator
                .AddFluentValidation(fluentValidationMvcConfiguration => fluentValidationMvcConfiguration.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            // ******************* AutoMapper for response/domain/DTO objects*******************
            // ******************* AutoMapper for response/domain/DTO objects*******************
            // ******************* AutoMapper for response/domain/DTO objects*******************
            services.AddAutoMapper(typeof(Startup));
        }
    }
}
