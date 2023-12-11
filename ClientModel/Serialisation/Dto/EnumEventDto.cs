using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AutomateDesign.Client.Model.Serialisation.Dto.EventReferenceDto;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Un état défini par l'utilisateur à transférer vers/depuis le serveur.
    /// </summary>
    internal class EnumEventDto
    {
        /// <summary>
        /// Le nom de l'évènement.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// L'identifiant de l'évènement.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="evt">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les informations du <paramref name="evt"/>.</returns>
        public static EnumEventDto MapFromModel(EnumEvent evt)
        {
            return new EnumEventDto
            {
                Id = evt.Id,
                Name = evt.Name,
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public EnumEvent MapToModel()
        {
            return new EnumEvent(this.Id, this.Name);
        }
    }
}
