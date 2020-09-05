using System.Collections.Generic;
using System.Linq;
using SalonAPI.Domain;

namespace SalonAPI.Extensions
{
    public static class UserService
    {
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static User WithoutPassword(this User user) {
            user.Password = null;
            return user;
        }
    }
}