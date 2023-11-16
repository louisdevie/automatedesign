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

            }
        }

        public override EditorState Next(EditorEvent evt)
        {
            switch (evt)
            {
                default:
                    return this;
            }
        }
    }
}
