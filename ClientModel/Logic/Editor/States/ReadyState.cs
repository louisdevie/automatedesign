namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// Aucune action n'est en cours.
    /// </summary>
    public class ReadyState : EditorState
    {
        public override string StatusMessage => "Prêt";

        public override void Action(EditorEvent e, EditorContext ctx)
        {
            switch (e)
            {
                case EditorEvent.BeginCreatingState:
                    ctx.Mode = EditorMode.Place;
                    ctx.EditorUI.ShowStateToAdd();
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            switch (e)
            {
                case EditorEvent.BeginCreatingState:
                    return new NewStatePlacementState();

                default:
                    return this;
            }
        }
    }
}
