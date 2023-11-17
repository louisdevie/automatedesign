using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Serialisation.Dto
{
    /// <summary>
    /// Une transition à transférer depuis/vers le serveur.
    /// </summary>
    internal class TransitionDto
    {
        /// <inheritdoc cref="Transition.Id"/>
        public int Id { get; set; }

        /// <inheritdoc cref="Transition.Start"/>
        public int Start { get; set; }

        /// <inheritdoc cref="Transition.End"/>
        public int End { get; set; }

        /// <inheritdoc cref="Transition.TriggeredBy"/>
        public EventDto TriggeredBy { get; set; } = new();

        /// <summary>
        /// Crée un DTO à partir du modèle.
        /// </summary>
        /// <param name="transition">Le modèle à sérialiser.</param>
        /// <returns>Un DTO contenant les unformation du <paramref name="transition"/>.</returns>
        public static TransitionDto MapFromModel(Transition transition)
        {
            return new TransitionDto
            {
                Id = transition.Id,
                Start = transition.Start.Id,
                End = transition.End.Id,
                TriggeredBy = EventDto.MapFromModel(transition.TriggeredBy),
            };
        }

        /// <summary>
        /// Crée un modèle à partir de ce DTO.
        /// </summary>
        /// <param name="document">Le document qui va contenir la transition.</param>
        /// <returns>Un nouveau modèle avec les informations de ce DTO.</returns>
        public Transition MapToModel(Document document)
        {
            return new Transition(
                this.Id,
                document.FindState(this.Start)!,
                document.FindState(this.End)!,
                this.TriggeredBy.MapToModel()
            );
        }
    }
}