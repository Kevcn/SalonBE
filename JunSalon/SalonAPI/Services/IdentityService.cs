using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalonAPI.Domain;
using SalonAPI.Extensions;

namespace SalonAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = 1, FirstName = "Jun", LastName = "Salon", Username = "JunSalon", Password = "JSManchester2020"
            }
        };

        public async Task<User> Authenticate(string username, string password)
        {
            var user = await Task.Run(() =>
                _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await Task.Run(() => _users.WithoutPasswords());
        }
    }
}