using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    public class UsersClient: Client
    {
        public async Task<int> SignUpAsync(string email, string password)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            SignUpReply reply = await client.SignUpAsync(
                new SignUpRequest
                {
                    Email = email,
                    Password = password
                }
            );

            return reply.UserId;
        }

        public async Task VerifyEmailAsync(int userId, uint verificationCode)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            await client.VerifyEmailAsync(
                new VerificationRequest
                {
                    UserId = userId,
                    SecretCode = verificationCode
                }
            );
        }
    }
}
