using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Protos;

namespace AutomateDesign.Client.Model.Network
{
    /// <summary>
    /// Interface du service utilisateurs.
    /// </summary>
    public interface IUsersClient
    {
        /// <summary>
        /// Change le mot de passe d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur qui change son mot de passe.</param>
        /// <param name="newPassword">Le nouveau mot de passe de l'utilisateur.</param>
        /// <param name="currentPassword">Le mot de passe actuel de l'utilisateur.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task ChangePasswordAsync(int userId, string newPassword, string currentPassword);

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur qui change son mot de passe.</param>
        /// <param name="newPassword">Le nouveau mot de passe de l'utilisateur.</param>
        /// <param name="resetCode">Le code secret à utiliser pour réinitialiser le mot de passe.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task ChangePasswordWithResetCodeAsync(int userId, string newPassword, uint resetCode);

        /// <summary>
        /// Vérifie si un code de réinitialisation est encore valide.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur qui veut utiliser le code.</param>
        /// <param name="resetCode">Le code secret à tester.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task CheckResetCodeAsync(int userId, uint resetCode);

        /// <summary>
        /// Déconnecte un utilisateur.
        /// </summary>
        /// <param name="session">La session actuelle contenant les informations de l'utilisateur.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task DisconnectAsync(Session session);

        /// <summary>
        /// Demande de réinitialiser un mot de passe.
        /// </summary>
        /// <param name="email">L'adresse mail de l'utilisateur qui a effectué la demande.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task<int> ResetPasswordAsync(string email);

        /// <summary>
        /// Ouvre une session pour un utilisateur.
        /// </summary>
        /// <param name="email">L'adresse mail utilisée pour se connecter.</param>
        /// <param name="password">Le mot de passe utilisé pour se connecter.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task<Session> SignInAsync(string email, string password);

        /// <summary>
        /// Inscrit un nouvel utilisateur.
        /// </summary>
        /// <param name="email">L'adresse mail utilisée pour s'inscrire.</param>
        /// <param name="password">Le mot de passe utilisé pour s'inscrire.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task<int> SignUpAsync(string email, string password);

        /// <summary>
        /// Active le compte d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur à vérifier.</param>
        /// <param name="verificationCode">Le code vérification utilisé pour activer le compte.</param>
        /// <returns>Une tâche représentant l'opération.</returns>
        Task VerifyUserAsync(int userId, uint verificationCode);
    }
}