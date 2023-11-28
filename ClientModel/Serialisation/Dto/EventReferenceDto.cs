using AutomateDesign.Core.Documents;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Une référence vers un évènement déclaré ailleurs dans l'automate sérialisé.
    /// </summary>
    internal class EventReferenceDto
    {
        public enum EventType { DefaultEvent, EnumEvent }

        /// <summary>
        /// Le type d'évènement.
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// L'identifiant de l'évènement s'il en a un.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Id { get; set; }

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="evt">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les informations du <paramref name="evt"/>.</returns>
        public static EventReferenceDto MapFromModel(Event evt)
        {
            return evt switch
            {
                DefaultEvent defaultEvent => new EventReferenceDto
                {
                    Type = EventType.DefaultEvent,
                    Id = null,
                },

                EnumEvent enumEvent => new EventReferenceDto
                {
                    Type = EventType.EnumEvent,
                    Id = enumEvent.Id,
                },

                _ => throw new InvalidOperationException($"Le type d'évènement {evt.GetType()} n'est pas pris en charge.")
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary> 
        /// <param name="document">Le document qui va contenir l'évènement.</param>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public Event MapToModel(Document document)
        {
            Event result;
            switch (this.Type)
            {
                case EventType.DefaultEvent:
                    result = new DefaultEvent();
                    break;

                case EventType.EnumEvent:
                    result = document.FindEnumEvent(this.Id!.Value)!;
                    break;

                default:
                    throw new InvalidOperationException($"Le type d'évènement {this.Type} n'est pas pris en charge.");
            }
            return result;
        }
    }
}