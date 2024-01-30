using AutomateDesign.Client.Model.Logic.Exceptions;
using AutomateDesign.Client.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de base pour les opération demandant à saisir un nouveau mot de passe.
    /// </summary>
    public abstract class NewPasswordBaseViewModel : UsersBaseViewModel
    {
        private PasswordBoxBinding passwordInput, passwordAgainInput;

        /// <summary>
        /// Le premier mot de passe.
        /// </summary>
        public PasswordBoxBinding Password => this.passwordInput;

        /// <summary>
        /// Le mot de passe répété.
        /// </summary>
        public PasswordBoxBinding PasswordAgain => this.passwordAgainInput;

        public NewPasswordBaseViewModel()
        {
            this.passwordInput = new();
            this.passwordInput.PasswordChanged += this.AnyPasswordChanged;
            this.passwordAgainInput = new();
            this.passwordAgainInput.PasswordChanged += this.AnyPasswordChanged;
        }

        private void AnyPasswordChanged(PasswordBoxBinding binding)
        {
            this.NotifyPropertyChanged(nameof(PasswordsNotEmpty));
            this.NotifyPropertyChanged(nameof(PasswordsMatch));
            if (binding == this.Password) this.NotifyPropertyChanged(nameof(PasswordValue));
        }

        /// <summary>
        /// Indique si les mots de passe ont bien été saisis.
        /// </summary>
        public bool PasswordsNotEmpty => !string.IsNullOrEmpty(this.passwordInput.Password)
                                      && !string.IsNullOrEmpty(this.passwordAgainInput.Password);

        /// <summary>
        /// Indique si les mots de passe correspondent.
        /// </summary>
        public bool PasswordsMatch => this.passwordInput.Password == this.passwordAgainInput.Password;

        /// <summary>
        /// La valeur du premier mot de passe.
        /// </summary>
        public string PasswordValue => this.passwordInput.Password;

        public void ThrowIfInputsAreInvalid()
        {
            if (!this.PasswordsNotEmpty)
            {
                throw new InvalidInputsException("Veuillez saisir un mot de passe");
            }
            else if (!this.PasswordsMatch)
            {
                throw new InvalidInputsException("Les mots de passe ne correspondent pas");
            }
        }
    }
}
