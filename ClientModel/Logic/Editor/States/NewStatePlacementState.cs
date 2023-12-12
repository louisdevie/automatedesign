namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// Un nouvel état va être ajouté.
    /// </summary>
    public class NewStatePlacementState : EditorState
    {
        public override string StatusMessage => "Cliquez pour ajouter l'état";

        public override void Action(EditorEvent evt, EditorContext ctx)
        {
            switch (evt)
            {
                case EditorEvent.FinishCreatingState finishCreatingState:
                    ctx.AddState(finishCreatingState.position);
                    ctx.Mode = EditorMode.Move;
                    break;
            }
        }

        public override EditorState Next(EditorEvent evt)
        {
            EditorState nextState = this;

            switch (evt)
            {
                case EditorEvent.FinishCreatingState:
                    nextState = new ReadyState();
                    break;
            }

            return nextState;
        }
    }
}
