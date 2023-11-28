using AutomateDesign.Client.DependencyInjection;
using AutomateDesign.Client.Model.Network;

namespace AutomateDesign.Client.ViewModel.Users
{
    /// <summary>
    /// Le modèle-vue de base pour la gestion des utilisateurs.
    /// </summary>
    public class UsersBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Un client réseau pour effectuer des requêtes liées aux utilisateurs.
        /// </summary>
        protected static IUsersClient Users => DependencyContainer.Current.GetImplementation<IUsersClient>();
    }
}
