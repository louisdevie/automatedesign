using AutomateDesign.Core.Users;
using Grpc.Core;

namespace AutomateDesign.Server.Middleware.Authentication
{
    public static class ServerCallContextExtensions
    {
        /// <summary>
        /// Récupère l'utilisateur qui a effectué la requête.
        /// </summary>
        /// <returns>L'utilisateur si la requête était bien authentifiée.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static User RequireUser(this ServerCallContext @this)
        {
            return (@this.GetHttpContext().Items["User"] as User)
                ?? throw new InvalidOperationException("No user associated with this request.");
        }
    }
}
