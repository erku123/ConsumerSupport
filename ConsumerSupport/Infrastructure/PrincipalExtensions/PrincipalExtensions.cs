using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ConsumerSupport.Infrastructure.PrincipalExtensions
{
    public static class PrincipalExtensions
    {

        public static string GetUserId(this IPrincipal principal)
        {
            var claimsIdentity = (ClaimsIdentity) principal.Identity;

            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
