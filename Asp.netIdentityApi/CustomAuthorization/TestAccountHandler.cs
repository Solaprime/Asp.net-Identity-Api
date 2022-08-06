using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.CustomAuthorization
{
    public class TestAccountHandler : AuthorizationHandler<TestAccountRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestAccountRequirement requirement)
        {
            var userEmailAddress = context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if (userEmailAddress.StartsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            //var userEmailAddress = context.User?.FindFirst("Email");
            //if (userEmailAddress.ToString().StartsWith(requirement.DomainName))
            //{
            //    context.Succeed(requirement);
            //    return Task.CompletedTask;
            //}

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
