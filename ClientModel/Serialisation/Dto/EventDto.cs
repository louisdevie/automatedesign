using AutomateDesign.Core.Documents;
using System.Text.Json.Serialization;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    public class EventDto
    {
        public enum EventType { DefaultEvent, EnumEvent }

        /// <summary>
        /// Le type d'évènement.
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// Le nom de l'évènement s'il en a un.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Name { get; set; }

        /// <summary>
        /// L'identifiant de l'évènement s'il en a un.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Id { get; private set; }

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="evt">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les informations du <paramref name="evt"/>.</returns>
        internal static EventDto MapFromModel(Event evt)
        {
            return evt switch
            {
                DefaultEvent defaultEvent => new EventDto
                {
                    Type = EventType.DefaultEvent,
                    Id = null,
                    Name = null,
                },

                EnumEvent enumEvent => new EventDto
                {
                    Type = EventType.EnumEvent,
                    Id = enumEvent.Id,
                    Name = enumEvent.Name
                },

                _ => throw new InvalidOperationException($"Le type d'évènement {evt.GetType()} n'est pas pris en charge.")
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary> 
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        internal Event MapToModel()
        {
            return this.Type switch
            {
                EventType.DefaultEvent => new DefaultEvent(),

                EventType.EnumEvent => new EnumEvent(this.Id ?? 0, this.Name ?? ""),

                _ => throw new InvalidOperationException($"Le type d'évènement {this.Type} n'est pas pris en charge.")
            };
        }
    }
}