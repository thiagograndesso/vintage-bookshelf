using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace VintageBookshelf.UI.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequiredClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }
}