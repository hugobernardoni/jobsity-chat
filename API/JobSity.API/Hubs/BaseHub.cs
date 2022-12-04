using JobSity.API.Core.JWT;
using Microsoft.AspNetCore.SignalR;

namespace JobSity.API.Hubs
{
    public class BaseHub : Hub
    {
        public JwtCurrentUserInfo CurrentUser => new JwtCurrentUserInfo(this.Context.User);
    }
}
