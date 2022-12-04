using System.Security.Claims;

namespace JobSity.API.Core.JWT
{
    public class JwtCurrentUserInfo
    {
        private ClaimsPrincipal _user;

        public JwtCurrentUserInfo(ClaimsPrincipal user)
        {
            _user = user;

        }

        public string UserId => !_user.Claims.Any() ? string.Empty : _user.Claims.ToList().FirstOrDefault(x => x.Type == "UserId").Value;
        public string Username => !_user.Claims.Any() ? "" : _user.Claims.ToList().FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
    }
}
