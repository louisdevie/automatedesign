using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    public interface IUsersClient
    {
        Task ChangePasswordAsync(int userId, string newPassword, string currentPassword);
        Task ChangePasswordWithResetCodeAsync(int userId, string newPassword, uint resetCode);
        Task CheckResetCodeAsync(int userId, uint resetCode);
        Task DisconnectAsync(string token);
        Task<int> ResetPasswordAsync(string email);
        Task<SignInReply> SignInAsync(string email, string password);
        Task<int> SignUpAsync(string email, string password);
        Task VerifyUserAsync(int userId, uint verificationCode);
    }
}