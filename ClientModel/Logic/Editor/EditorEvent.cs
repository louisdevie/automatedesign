using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Logic.Editor
{
    /// <summary>
    /// Une énumération « riche » qui liste les évènements possible d'édition des automates.
    /// </summary>
    public abstract record EditorEvent
    {
        /// <summary>
        /// Un état commence à être ajouté au diagramme, mais il ne l'a pas encore confirmé.
        /// </summary>
        public record BeginCreatingState() : EditorEvent;

        /// <summary>
        /// Un état est ajouté au diagramme.
        /// </summary>
        public record FinishCreatingState() : EditorEvent;

        /// <summary>
        /// Une transition commence à être ajoutée au diagramme.
        /// </summary>
        public record BeginCreatingTransition() : EditorEvent;

        /// <summary>
        /// Un état a été sélectionné.
        /// </summary>
        /// <param name="state">L'état sélectionné.</param>
        public record SelectState(State state) : EditorEvent;

        /// <summary>
        /// Un évènement doit être ajouté à l'automate.
        /// </summary>
        public record CreateEnumEvent() : EditorEvent;

        /// <summary>
        /// Arrête l'opération en cours.
        /// </summary>
        public record Cancel() : EditorEvent;
    }
}
