using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutomateDesign.Client.View.Navigation
{
    /// <summary>
    /// Un élément de l'UI apte à contenir des vues.
    /// </summary>
    public interface INavigationContainer
    {
        /// <summary>
        /// La fenêtre parente au conteneur.
        /// </summary>
        Window ParentWindow { get; }

        /// <summary>
        /// Applique les préférences de la vue actuelle. Le conteneur a le choix de suivre les préférences ou non.
        /// </summary>
        /// <param name="preferences">Les préférences demandées par la vue.</param>
        void ApplyPreferences(WindowPreferences preferences);

        /// <summary>
        /// Change la vue contenue.
        /// </summary>
        /// <param name="value">La nouvelle vue à afficher.</param>
        void ChangeContent(INavigable value);
    }
}
