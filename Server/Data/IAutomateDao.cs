using AutomateDesign.Core.Documents;
using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    public interface IAutomateDao
    {
        /// <summary>
        /// Ajoute le document fournit a la bdd
        /// </summary>
        /// <param name="document">le document a ajouter</param>
        /// <returns>le document fournit avec son identifiant</returns>
        public DocumentCrypte Create(DocumentCrypte document);

        /// <summary>
        /// Renvoie le document correspondant a l'identifiant fournit
        /// </summary>
        /// <param name="id">identifiant du document cherche</param>
        /// <returns></returns>
        public DocumentCrypte ReadById(int id);

        /// <summary>
        /// Met a jour dans la bdd le document founit
        /// </summary>
        /// <param name="document"></param>
        public void Update(DocumentCrypte document);

        /// <summary>
        /// Supprime de la bdd le document correspondant à l'identifiant fournit
        /// </summary>
        /// <param name="id">l'identifiant du document a fournit</param>
        public void Delete(int id);
    }
}
