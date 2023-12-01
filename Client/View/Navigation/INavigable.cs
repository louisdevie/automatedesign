using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.View.Navigation
{
    /// <summary>
    /// Une vue qui peut être untilisée dans un conteneur de navigation.
    /// </summary>
    public interface INavigable
    {
        /// <summary>
        /// Les préférences de la vue pour son conteneur.
        /// Le conteneur est toujours libre d'ignorer ces préférences.
        /// </summary>
        WindowPreferences Preferences { get; }

        /// <summary>
        /// Reçois un navigateur à utiliser pour passer sur d'autres vues.
        /// </summary>
        /// <param name="navigator">Le navigateur à utiliser.</param>
        void UseNavigator(Navigator navigator);

        /// <summary>
        /// Appelée quand une autre vue demande à naviguer vers celle-ci.
        /// </summary>
        /// <param name="clearedHistory">Si <see langword="true"/>, tout l'historique avant cette page à été effacé et on ne plus revenir en arrière.</param>
        void OnNavigatedToThis(bool clearedHistory);

        /// <summary>
        /// Appelée quand une autre vue reviens sur celle-ci.
        /// </summary>
        void OnWentBackToThis();
    }
}
