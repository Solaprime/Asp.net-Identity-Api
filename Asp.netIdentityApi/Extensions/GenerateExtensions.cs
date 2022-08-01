using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Extensions
{
    // an extension Method, we use httpContex to find user Email by JwtToken
    //   note in the Claim vibes, the email is passed as a String so if we search we passed the Email in the returnType
    public static class GenerateExtensions
    {
        public static string GetUserEmailFromToken(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == "Email").Value;
         // return httpContext.User.Claims.
        }
        
    } 
}
