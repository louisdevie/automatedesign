using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Des métadonnées d'automate à transférer depuis/vers le serveur.
    /// </summary>
    internal class DocumentHeaderDto
    {
        /// <inheritdoc cref="DocumentHeader.Name"/>
        public string Name { get; set; } = "";

        /// <inheritdoc cref="DocumentHeader.LastModificationdate"/>
        public DateTime LastModificationDate { get; set; }

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="header">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les unformation du <paramref name="header"/>.</returns>
        public static DocumentHeaderDto MapFromModel(DocumentHeader header)
        {
            return new DocumentHeaderDto
            {
                Name = header.Name,
                LastModificationDate = header.LastModificationdate
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public DocumentHeader MapToModel()
        {
            return new DocumentHeader(this.Name, this.LastModificationDate);
        }
    }
}
