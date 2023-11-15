﻿using AutomateDesign.Client.Model.Logic.Verifications;
using AutomateDesign.Client.Model.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de l'inscription.
    /// </summary>
    public class SignUpViewModel : NewPasswordBaseViewModel
    {
        private string email;

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
            int userId = await Users.SignUpAsync(this.email, this.Password.Password);
            return new SignUpEmailVerificationViewModel(
                new SignUpEmailVerification(userId),
                this.email,
                this.Password.Password
            );
        }
    }
}
