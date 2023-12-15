namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// Aucune action n'est en cours.
    /// </summary>
    public class ReadyState : EditorState
    {
        public override string StatusMessage => "";

        public override void Action(EditorEvent e, EditorContext ctx)
        {
            switch (e)
            {
                case EditorEvent.BeginCreatingState:
                    ctx.Mode = EditorMode.Place;
                    ctx.EditorUI.ShowStateToAdd();
                    break;

                case EditorEvent.BeginCreatingTransition:
                    ctx.Mode = EditorMode.Select;
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            EditorState nextState = this;

            switch (e)
            {
                case EditorEvent.BeginCreatingState:
                    return new NewStatePlacementState();

                case EditorEvent.BeginCreatingTransition:
                    return new NewTransitionSelectStartState();
            }

            return nextState;
        }
    }
}
