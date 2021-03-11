using System;
using Microsoft.AspNetCore.Http;

namespace VintageBookshelf.Api.Extensions
{
    public static class CustomAuthorizationExtensions
    {
        public static bool ValidateUserClaims(this HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                   context.User.HasClaim(claimName, claimValue);
        }
    }
}