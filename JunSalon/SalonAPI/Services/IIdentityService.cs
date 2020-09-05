using System.Collections.Generic;
using System.Threading.Tasks;
using SalonAPI.Domain;

namespace SalonAPI.Services
{
    public interface IIdentityService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
    }
}