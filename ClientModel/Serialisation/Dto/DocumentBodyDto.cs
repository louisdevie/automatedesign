using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Un automate à transférer depuis/vers le serveur.
    /// </summary>
    internal class DocumentBodyDto
    {
        /// <inheritdoc cref="Document.States"/>
        public List<StateDto> States { get; set; } = new();

        /// <inheritdoc cref="Document.Transitions"/>
        public List<TransitionDto> Transitions { get; set; } = new();

        /// <inheritdoc cref="Document.Events"/>
        public List<EnumEventDto> Events { get; set; } = new();

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="document">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les unformation du <paramref name="document"/>.</returns>
        public static DocumentBodyDto MapFromModel(Document document)
        {
            return new DocumentBodyDto
            {
                States = document.States.Select(StateDto.MapFromModel).ToList(),
                Events = document.Events.Select(EnumEventDto.MapFromModel).ToList(),
                Transitions = document.Transitions.Select(TransitionDto.MapFromModel).ToList(),
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <param name="header">L'en-tête du document.</param>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public Document MapToModel(DocumentHeader? header = null)
        {
            // utilise le constructeur avec en-tête si le paramètre est présent
            Document result = header == null ? new() : new(header);

            result.AddStates(this.States.Select(state => state.MapToModel(result)));
            result.AddEvents(this.Events.Select(evt => evt.MapToModel()));
            result.AddTransitions(this.Transitions.Select(transition => transition.MapToModel(result)));

            return result;
        }
    }
}
