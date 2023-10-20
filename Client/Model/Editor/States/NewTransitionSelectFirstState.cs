namespace AutomateDesign.Client.Model.Editor.States
{
    internal class NewTransitionSelectFirstState : EditorState
    {
        public override string Description => "Sélectionnez un état de départ";

        public override void Action(EditorEvent e, EditorContext ctx)
        {
            switch (e)
            {
                case EditorEvent.SelectState selectState:
                    ctx.StartDrawingTransition(selectState.state);
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            switch (e)
            {
                case EditorEvent.SelectState selectState:
                    return new NewTransitionSelectSecondState(selectState.state);

                default:
                    return this;
            }
        }
    }
}