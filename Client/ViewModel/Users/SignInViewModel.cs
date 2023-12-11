using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de la connexion.
    /// </summary>
    public class SignInViewModel : UsersBaseViewModel
    {
        private string email;
        private PasswordBoxBinding passwordInput;

        /// <summary>
        /// L'adresse mail à utiliser pour se connecter.
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

        /// <summary>
        /// Le mot de passe à utiliser pour se connecter.
        /// </summary>
        public PasswordBoxBinding Password => this.passwordInput;

        public SignInViewModel()
        {
            this.passwordInput = new();
            this.email = string.Empty;
        }

        /// <summary>
        /// Se connecte avec les informations de <see cref="Email"/> et <see cref="Password"/>.
        /// </summary>
        /// <returns>Une tâche représentant l'opération, qui termine avec la session ouverte.</returns>
        public async Task<Session> SignInAsync()
        {
            return await Users.SignInAsync(this.email, this.passwordInput.Password);
        }

        public async Task<SignUpEmailVerificationViewModel> RetryEmailVerificationAsync()
        {
            int userId = await Users.SignUpAsync(this.email, this.Password.Password);
            return new SignUpEmailVerificationViewModel(
                new SignUpEmailVerification(userId),
                this.email,
                this.Password.Password
            );
        }

        /// <summary>
        /// Efface le contenu du champ de mot de passe.
        /// </summary>
        public void ClearPassword()
        {
            this.passwordInput.Password = string.Empty;
        }
    }
}
