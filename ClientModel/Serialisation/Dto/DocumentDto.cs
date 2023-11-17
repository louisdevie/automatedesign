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
    internal class DocumentDto
    {
        /// <inheritdoc cref="Document.Header"/>
        public DocumentHeaderDto Header { get; set; } = new();

        /// <inheritdoc cref="Document.States"/>
        public List<StateDto> States { get; set; } = new();

        /// <inheritdoc cref="Document.Transitions"/>
        public List<TransitionDto> Transitions { get; set; } = new();

        /// <inheritdoc cref="Document.Events"/>
        public List<EventDto> Events { get; set; } = new();

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="document">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les unformation du <paramref name="document"/>.</returns>
        public static DocumentDto MapFromModel(Document document)
        {
            return new DocumentDto
            {
                Header = DocumentHeaderDto.MapFromModel(document.Header),
                States = document.States.Select(StateDto.MapFromModel).ToList(),
                Events = document.Events.Select(EventDto.MapFromModel).ToList(),
                Transitions = document.Transitions.Select(TransitionDto.MapFromModel).ToList(),
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public Document MapToModel()
        {
            return new Document();
        }
    }
}
