using AutomateDesign.Core.Users;

namespace AutomateDesign.Server.Data
{
    public interface IUserDao
    {
        /// <summary>
        /// Ajout de l'utilisateur fournit dans la bdd
        /// </summary>
        /// <param name="user">l'utilisateur a ajoute</param>
        /// <returns>l'utilisateur avec son identifiant</returns>
        public User Create(User user);

        /// <summary>
        /// Renvoie l'utilisateur correspondant a l'identifiant fournit
        /// </summary>
        /// <param name="id">l'id de l'utilisateur a cherche</param>
        /// <returns>l'utilisateur</returns>
        public User ReadById(int id);

        /// <summary>
        /// Renvoie l'utilisateur correspondant a l'adresse mail demande
        /// </summary>
        /// <param name="address">adresse mail de l'utilisateur</param>
        /// <returns>l'utilisateur</returns>
        /// <exception cref="ResourceNotFoundException"></exception>
        public User ReadByEmail(string address);

        /// <summary>
        /// Met a jour dans la bdd l'utilisateur fournit
        /// </summary>
        /// <param name="user">l'utilisateur a mettre a jour</param>
        public void Update(User user);

        /// <summary>
        /// Supprime l'utilisateur correspondant a l'id de la bdd
        /// </summary>
        /// <param name="id">l'id de l'utilisateur</param>
        public void Delete(int id);
    }
}
