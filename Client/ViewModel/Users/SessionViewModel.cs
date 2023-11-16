using AutomateDesign.Client.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue d'une <see cref="Session"/>.
    /// </summary>
    public class SessionViewModel : UsersBaseViewModel
    {
        private Session session;

        /// <summary>
        /// L'addresse mail de l'utilisateur.
        /// </summary>
        public string UserEmail => this.session.UserEmail;

        public SessionViewModel(Session session)
        {
            this.session = session;
        }

        /// <summary>
        /// Se déconnecte de cette session.
        /// </summary>
        /// <returns>Une tâche représentant l'opération.</returns>
        public async Task SignOutAsync()
        {
            await Users.DisconnectAsync(this.session.Token);
        }

        /// <summary>
        /// Crée un modèle-vue de changement de mot de passe pour l'utilisateur connecté.
        /// </summary>
        /// <returns>Un nouveau <see cref="ChangePasswordViewModel"/>.</returns>
        public ChangePasswordViewModel ChangePassword()
        {
            return new ChangePasswordViewModel(this.session.UserId);
        }
    }
}
