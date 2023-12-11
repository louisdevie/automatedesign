using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Un état à transférer depuis/vers le serveur.
    /// </summary>
    internal class StateDto
    {
        /// <inheritdoc cref="State.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="State.Name"/>
        public string Name { get; set; } = "";

        /// <inheritdoc cref="State.Kind"/>
        public StateKind Kind { get; set; }

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="state">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les unformation du <paramref name="state"/>.</returns>
        public static StateDto MapFromModel(State state)
        {
            return new StateDto
            {
                Id = state.Id,
                Name = state.Name,
                Kind = state.Kind,
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <param name="document">Le document qui va contenir l'état.</param>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public State MapToModel(Document document)
        {
            return new State(document, this.Id, this.Name, this.Kind);
        }
    }
}
