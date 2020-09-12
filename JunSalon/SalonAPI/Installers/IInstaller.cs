using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SalonAPI.Installers
{
    internal interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration config);
    }
}