using AutomateDesign.Protos;
using AutomateDesign.Client.Model.Logic;

namespace AutomateDesign.Client.Model.Network
{
    /// <summary>
    /// Une implémentation de <see cref="IUsersClient"/> qui utilise le service gRPC.
    /// </summary>
    public class UsersClient : Client, IUsersClient
    {
        public async Task<int> SignUpAsync(string email, string password)
        {
            var client = new Users.UsersClient(this.Channel);

            UserIdOnly reply = await client.SignUpAsync(
                new EmailAndPassword
                {
                    Email = email,
                    Password = password
                }
                
            );

            return reply.UserId;
        }

        public async Task VerifyUserAsync(int userId, uint verificationCode)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.VerifyUserAsync(
                new VerificationRequest
                {
                    UserId = userId,
                    SecretCode = verificationCode
                }
            );
        }

        public async Task<Session> SignInAsync(string email, string password)
        {
            var client = new Users.UsersClient(this.Channel);

            var response = await client.SignInAsync(
                new EmailAndPassword
                {
                    Email = email,
                    Password = password
                }
            );

            return new Session(response.Token, response.UserId, email, password);
        }

        public async Task ChangePasswordAsync(int userId, string newPassword, string currentPassword)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.ChangePasswordAsync(
                new PasswordChangeRequest
                {
                    UserId = userId,
                    NewPassword = newPassword,
                    CurrentPassword = currentPassword
                }
            );
        }

        public async Task ChangePasswordWithResetCodeAsync(int userId, string newPassword, uint resetCode)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.ChangePasswordAsync(
                new PasswordChangeRequest
                {
                    UserId = userId,
                    NewPassword = newPassword,
                    SecretCode = resetCode
                }
            );
        }

        public async Task<int> ResetPasswordAsync(string email)
        {
            var client = new Users.UsersClient(this.Channel);

            UserIdOnly reply = await client.ResetPasswordAsync(
                new PasswordResetRequest { Email = email }
            );

            return reply.UserId;
        }

        public async Task CheckResetCodeAsync(int userId, uint resetCode)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.CheckResetCodeAsync(
                new VerificationRequest
                {
                    UserId = userId,
                    SecretCode = resetCode
                }
            );
        }

        public async Task DisconnectAsync(Session session)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.DisconnectAsync(
                new SessionUser { Session = session.Token }
            );
        }

        public async Task DeleteAutomateAsync(int idAutomate)
        {
            var client = new Users.UsersClient(this.Channel);

            await client.DeleteAutomateAsync(
                new AutomateId { Id = idAutomate }
            );
        }

    }
}
