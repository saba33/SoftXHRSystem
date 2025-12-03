using System.Security.Claims;

namespace HRSystem.API.Extensions
{
    public static class ClaimsExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst("userId")
                        ?? user.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return -1;

            return int.TryParse(claim.Value, out var id) ? id : -1;
        }
    }
}
