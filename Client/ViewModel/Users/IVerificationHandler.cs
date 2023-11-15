using AutomateDesign.Client.Model.Logic.Verifications;
using System.Reflection.Metadata;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Peut visiter une opération à vérifier.
    /// </summary>
    public interface IVerificationHandler
    {
        void Handle(SignUpEmailVerification verification);

        void Handle(PasswordResetVerification verification);
    }
}
