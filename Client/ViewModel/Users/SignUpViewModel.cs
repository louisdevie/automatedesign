using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using AutomateDesign.Client.Model.Logic.Exceptions;
using AutomateDesign.Client.View.Helpers;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de l'inscription.
    /// </summary>
    public class SignUpViewModel : NewPasswordBaseViewModel
    {
        private string email;
        private bool warningRead;

        /// <summary>
        /// L'adresse mail de l'utilisateur.
        /// </summary>
        public string Email
        {
            get => this.email;
            set
            {
                this.email = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool WarningRead
        {
            get => this.warningRead;
            set
            {
                this.warningRead = value;
                this.NotifyPropertyChanged();
            }
        }

        public SignUpViewModel()
        {
            this.email = string.Empty;
        }

        /// <summary>
        /// Effectue la demande d'inscription.
        /// </summary>
        /// <returns>Une tâche représentant l'opération, qui termine avec le modèle-vue de la vérification à effectuer ensuite.</returns>
        public async Task<SignUpEmailVerificationViewModel> SignUpAsync()
        {
            this.ThrowIfInputsAreInvalid();

            if (!this.WarningRead)
            {
                throw new InvalidInputsException(nameof(WarningRead));
            }
            else
            {
                int userId = await Users.SignUpAsync(this.email, this.Password.Password);
                return new SignUpEmailVerificationViewModel(
                    new SignUpEmailVerification(userId),
                    this.email,
                    this.Password.Password
                );
            }
        }
    }
}
