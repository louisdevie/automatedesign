using AutomateDesign.Client.Model.Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue d'un changement de mot de passe.
    /// </summary>
    public class ChangePasswordViewModel : NewPasswordBaseViewModel
    {
        private int userId;
        private PasswordBoxBinding currentPassword;

        /// <summary>
        /// Le mot de passe actuel de l'utilisateur.
        /// </summary>
        public PasswordBoxBinding CurrentPassword => this.currentPassword;

        /// <summary>
        /// Crée un nouveau <see cref="ChangePasswordViewModel"/> pour un utilisateur.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur.</param>
        public ChangePasswordViewModel(int userId)
        {
            this.userId = userId;
            this.currentPassword = new();
        }

        /// <summary>
        /// Effectue le changement de mot de passe.
        /// </summary>
        /// <returns>Une tâche qui représente l'opération.</returns>
        public async Task ChangePasswordAsync()
        {
            if (this.CurrentPassword.Password != string.Empty)
            {
                throw new InvalidInputsException("Veuillez saisir votre mot de passe actuel");
            }
            else if (!this.PasswordsNotEmpty)
            {
                throw new InvalidInputsException("Veuillez saisir un nouveau mot de passe");
            }
            else if (!this.PasswordsMatch)
            {
                throw new InvalidInputsException("Les mots de passe ne correspondent pas");
            }
            else
            {
                await Users.ChangePasswordAsync(this.userId, this.PasswordValue, this.currentPassword.Password);
            }
        }
    }
}
