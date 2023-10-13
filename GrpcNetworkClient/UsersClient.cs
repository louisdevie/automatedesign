﻿using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    public class UsersClient : Client
    {
        public async Task<int> SignUpAsync(string email, string password)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

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

        public async Task ChangePassword(int userId, string newPassword, string currentPassword)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            await client.ChangePasswordAsync(
                new PasswordChangeRequest
                {
                    UserId = userId,
                    NewPassword = newPassword,
                    CurrentPassword = currentPassword
                }
            );
        }

        public async Task ChangePasswordWithResetCode(int userId, string newPassword, uint resetCode)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            await client.ChangePasswordAsync(
                new PasswordChangeRequest
                {
                    UserId = userId,
                    NewPassword = newPassword,
                    SecretCode = resetCode
                }
            );
        }

        public async Task<int> ResetPassword(string email)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            UserIdOnly reply = await client.ResetPasswordAsync(
                new PasswordResetRequest { Email = email }
            );

            return reply.UserId;
        }

        public async Task truc(int userId, uint resetCode)
        {
            using var channel = this.OpenChannel();
            var client = new Users.UsersClient(channel);

            await client.CheckResetCodeAsync(
                new VerificationRequest
                {
                    UserId = userId,
                    SecretCode= resetCode
                }
            );
        }
    }
}
