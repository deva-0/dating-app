using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        /// <summary>
        /// Resolves username of currently authenticated user. (JWT)
        /// </summary>
        /// <param name="user">User associated with the executing action</param>
        /// <returns>Username of currently authenticated user</returns>
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}