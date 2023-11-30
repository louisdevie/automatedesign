using AutomateDesign.Core.Users;
using Grpc.Core;

namespace AutomateDesign.Server.Middleware.Authentication
{
    public static class ServerCallContextExtensions
    {
        public static User RequireUser(this ServerCallContext @this)
        {
            return (@this.GetHttpContext().Items["User"] as User)
                ?? throw new InvalidOperationException("No user associated with this request.");
        }
    }
}
