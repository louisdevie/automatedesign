using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await Users.ChangePasswordAsync(this.userId, this.PasswordValue, this.currentPassword.Password);
        }
    }
}
