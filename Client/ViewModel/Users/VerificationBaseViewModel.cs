using AutomateDesign.Client.Model.Logic.Verifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de base pour les vérifications avec un code à usage unique.
    /// </summary>
    public abstract class VerificationBaseViewModel : UsersBaseViewModel
    {
        private uint code;

        /// <summary>
        /// Le code de vérification.
        /// </summary>
        public string Code
        {
            get => this.code.ToString();
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.code = 0;
                }
                else
                {
                    this.code = uint.Parse(value);
                }
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// L'opération à vérifier.
        /// </summary>
        protected abstract Verification Verification { get; }

        /// <inheritdoc cref="Verification.Title"/>
        public string Title => this.Verification.Title;

        /// <inheritdoc cref="Verification.SuccessMessage"/>
        public string SuccessMessage => this.Verification.SuccessMessage;

        /// <inheritdoc cref="Verification.Continuation"/>
        public string Continuation => this.Verification.Continuation;

        /// <summary>
        /// La valeur du code de vérification.
        /// </summary>
        public uint CodeValue => this.code;

        /// <summary>
        /// Envoie une requête pour effectuer la vérification.
        /// </summary>
        /// <returns>Une tâche représentant l'opération.</returns>
        public async Task SendVerificationRequestAsync()
        {
            await this.Verification.SendVerificationRequest(Users, this.code);
        }

        /// <summary>
        /// Accepte un visiteur de <see cref="Model.Logic.Verifications.Verification"/>.
        /// </summary>
        /// <param name="handler">Le visiteur à accepter.</param>
        public abstract void DispatchHandler(IVerificationHandler handler);
    }
}
