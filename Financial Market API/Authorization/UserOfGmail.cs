using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Financial_Market_API.Authorization
{
    public class UserOfGmail : IAuthorizationRequirement
    {
        public string EmailProvider { get; set; }

        public UserOfGmail(string emailProvider)
        {
            EmailProvider = emailProvider;
        }
    }
}
