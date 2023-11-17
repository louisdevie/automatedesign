using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    public class AskForPasswordResetViewModel : UsersBaseViewModel
    {
        private string email;

        public string Email
        {
            get => this.email;
            set
            {
                this.email = value;
                this.NotifyPropertyChanged();
            }
        }

        public AskForPasswordResetViewModel()
        {
            this.email = string.Empty;
        }

        /// <summary>
        /// Demande à réinitialiser le mot de passe.
        /// </summary>
        /// <returns>Une tâche représentant l'opération, qui termine avec l'identifiant de l'utilisateur.</returns>
        public async Task<int> AskForPasswordResetAsync()
        {
            return await Users.ResetPasswordAsync(this.email);
        }
    }
}
