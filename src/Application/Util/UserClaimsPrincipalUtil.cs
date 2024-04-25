using shortfy_api.src.Application.Exceptions;
using System.Security.Claims;

namespace shortfy_api.src.Application.Util
{
    public class UserClaimsPrincipalUtil
    {
        public static Guid GetId(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new TokenNotValidExpcetion();
            }
            var id = user?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(id))
            {
                throw new TokenNotValidExpcetion();
            }

            return Guid.Parse(id);
        }

        public static Guid? GetIdNullable(ClaimsPrincipal user)
        {
            if (user == null)
            {
                return null;
            }
            var id = user?.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return Guid.Parse(id);
        }
        public static string GetEmail(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new TokenNotValidExpcetion();
            }

            return user?.Claims.FirstOrDefault(c => c.Type.Contains("email"))?.Value ?? string.Empty;
        }
    }
}
