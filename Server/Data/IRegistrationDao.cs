using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface IRegistrationDao
    {
        /// <summary>
        /// Enregistre une demande d'inscription. Si l'utilisateur associé n'existe pas encore, il est créé en même temps.
        /// </summary>
        /// <param name="registration">La demande d'inscription à enregistrer.</param>
        public void Create(Registration registration);

        /// <summary>
        /// Récupère la demande d'inscription correspondant à un utilisateur.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur pour qui récupérer les informations.</param>
        public void Read(int userId);

        /// <summary>
        /// Supprimme une demande d'inscription.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur à qui correspond la demande d'inscription.</param>
        public void Delete(int userId);
    }
}
