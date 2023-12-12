namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// On attends que l'état de départ soit choisi pour ajouter une transition.
    /// </summary>
    internal class NewTransitionSelectStartState : EditorState
    {
        public override string StatusMessage => "Sélectionnez un état de départ";

        public override void Action(EditorEvent evt, EditorContext ctx)
        {
            switch (evt)
            {
            }
        }

        public override EditorState Next(EditorEvent evt)
        {
            switch (evt)
            {
                case EditorEvent.SelectState selectState:
                    return new NewTransitionSelectEndState(selectState.state);

                default:
                    return this;
            }
        }
    }
}