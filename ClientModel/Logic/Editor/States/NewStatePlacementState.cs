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
            throw new NotImplementedException();
        }

        public override EditorState Next(EditorEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}
