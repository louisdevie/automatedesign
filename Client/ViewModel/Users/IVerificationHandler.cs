using AutomateDesign.Client.Model.Logic.Verifications;
using System.Reflection.Metadata;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Peut visiter une opération à vérifier.
    /// </summary>
    public interface IVerificationHandler
    {
        /// <summary>
        /// Gère le cas ou l'opération est une <see cref="SignUpEmailVerification"/>.
        /// </summary>
        /// <param name="verification">La vérification visitée.</param>
        void Handle(SignUpEmailVerification verification);

        /// <summary>
        /// Gère le cas ou l'opération est une <see cref="PasswordResetVerification"/>.
        /// </summary>
        /// <param name="verification">La vérification visitée.</param>
        void Handle(PasswordResetVerification verification);
    }
}
