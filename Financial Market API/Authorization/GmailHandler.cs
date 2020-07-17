using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Financial_Market_API.Authorization
{
    public class GmailHandler : AuthorizationHandler<UserOfGmail>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOfGmail requirement)
        {
            var userEmail = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if (userEmail.EndsWith(requirement.EmailProvider))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
