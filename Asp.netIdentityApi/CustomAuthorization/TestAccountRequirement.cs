using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.CustomAuthorization
{
    //i want to add custom authorization that depends on if the user email begim with Test 
    public class TestAccountRequirement : IAuthorizationRequirement
    {
        public string DomainName { get;  }
        public TestAccountRequirement(string domainName)
        {
            DomainName = domainName;
        }
    } 
}
