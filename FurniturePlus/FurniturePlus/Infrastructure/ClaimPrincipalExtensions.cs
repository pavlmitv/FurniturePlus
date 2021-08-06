//Claim types represent part of the user information.
using System.Security.Claims;

namespace FurniturePlus.Infrastructure
{
    public static class ClaimPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
