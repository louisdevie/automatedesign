using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Editor
{
    public abstract record EditorEvent
    {
        public record CreateState() : EditorEvent;

        public record CreateTransition() : EditorEvent;

        public record SelectState(State state): EditorEvent;

    }
}
