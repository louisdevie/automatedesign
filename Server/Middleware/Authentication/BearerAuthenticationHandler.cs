using AutomateDesign.Core.Exceptions;
using AutomateDesign.Core.Users;
using AutomateDesign.Server.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace AutomateDesign.Server.Middleware.Authentication
{
    public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private ISessionDao sessions;

        public BearerAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ISessionDao sessions)
        : base(options, logger, encoder, clock)
        {
            this.sessions = sessions;
        }

        protected bool TryParseHeader(out string token)
        {
            token = "";

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authHeader.Scheme != "Bearer") return false;

                token = authHeader.Parameter!;

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
            }

            string token;
            if (!TryParseHeader(out token))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid header format"));
            }

            Session session;
            try
            {
                session = sessions.ReadByToken(token);
            }
            catch (ResourceNotFoundException)
            {
                return Task.FromResult(AuthenticateResult.Fail("Session not found"));
            }

            if (!session.Refresh())
            {
                sessions.Delete(session.Token);
                return Task.FromResult(AuthenticateResult.Fail("Session expired"));
            }
            sessions.UpdateLastUse(session);

            Context.Items.Add("Session", session);
            Context.Items.Add("User", session.User);

            return Task.FromResult(AuthenticateResult.Success(new EmptyTicket(Scheme)));
        }
    }

    public static class BearerAuthenticationExtensions
    {
        public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder)
        {
            return builder.AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("Bearer", null);
        }
    }
}
