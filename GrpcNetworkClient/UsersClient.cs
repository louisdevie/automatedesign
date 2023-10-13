using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    public class UsersClient : Client
    {
        public async Task<int> SignUpAsync(string email, string password)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            SignUpReply reply = await client.SignUpAsync(
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
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            await client.VerifyUserAsync(
                new VerificationRequest
                {
                    UserId = userId,
                    SecretCode = verificationCode
                }
            );
        }

        public async Task<string> SignInAsync(string email, string password)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            SignInReply reply = await client.SignInAsync(
                new EmailAndPassword
                {
                    Email = email,
                    Password = password
                }
            );

            return reply.Token;
        }
    }
}
