using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Editor.States
{
    internal class NewTransitionSelectSecondState : EditorState
    {
        private State firstState;

        public NewTransitionSelectSecondState(State firstState)
        {
            this.firstState = firstState;
        }

        public override string Description => "Sélectionnez un état d'arrivée";

        public override void Action(EditorEvent e, EditorContext ctx)
        {
            switch (e)
            {
                case EditorEvent.SelectState selectState:
                    ctx.ExitSelectionMode();
                    ctx.CreateTransition(this.firstState, selectState.state);
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            switch (e)
            {
                case EditorEvent.SelectState:
                    return new ReadyState();

                default:
                    return this;
            }
        }
    }
}