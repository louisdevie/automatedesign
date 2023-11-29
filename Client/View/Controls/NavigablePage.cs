using System;
using System.Windows.Controls;
using AutomateDesign.Client.View.Navigation;

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Une page qui implémente <see cref="INavigable"/> avec des opérations vides par défaut.
    /// </summary>
    public class NavigablePage : Page, INavigable
    {
        private Navigator? navigator;

        /// <summary>
        /// Le navigateur qui gère cette page. Cette propriété ne peut pas être utilisée avant d'avoir affiché la page dans un conteneur.
        /// </summary>
        protected Navigator Navigator => this.navigator ?? throw new InvalidOperationException("La page n'a jamais été affichée.");

        public void UseNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }

        /// <inheritdoc cref="INavigable.Preferences"/>
        /// <remarks>
        /// Par défaut, aucune préférence sur la fenêtre. Écrasez la propriété pour configurer les préférences.
        /// </remarks>
        public virtual WindowPreferences Preferences => new();

        /// <inheritdoc cref="INavigable.OnNavigatedToThis(bool)"/>
        /// <remarks>
        /// Ne fait rien par défaut. Écrasez cette méthode et <see cref="OnWentBackToThis"/> pour réagir au changement de page.
        /// </remarks>
        public virtual void OnNavigatedToThis(bool clearedHistory) { }

        /// <inheritdoc cref="INavigable.OnWentBackToThis()"/>
        /// <remarks>
        /// Ne fait rien par défaut. Écrasez cette méthode et <see cref="OnNavigatedToThis(bool)"/> pour réagir au changement de page.
        /// </remarks>
        public virtual void OnWentBackToThis() { }
    }
}
