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
                case EditorEvent.CreateState:
                    ctx.CreateState();
                    break;

                case EditorEvent.CreateTransition:
                    ctx.EnterSelectionMode();
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            switch (e)
            {
                case EditorEvent.CreateTransition:
                    return new NewTransitionSelectStartState();

                default:
                    return this;
            }
        }
    }
}
