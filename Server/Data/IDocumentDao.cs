using AutomateDesign.Core.Documents;
using System.Reflection.Metadata;

namespace AutomateDesign.Server.Data
{
    public interface IDocumentDao
    {
        /// <summary>
        /// Enregistre un nouveau document.
        /// </summary>
        /// <param name="userId">L'identifiant de l'utilisateur à qui appartient le document.</param>
        /// <param name="document">Le document chiffré.</param>
        /// <returns>L'identifiant du nouveau document.</returns>
        public int Create(int userId, byte[] document);

        /// <summary>
        /// Lis le document correspondant à l'identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] ReadById(int id);

        public 

        public void Update(int id, byte[] document);

        public void Delete(int id);
    }
}
