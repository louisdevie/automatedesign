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
                
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            switch (e)
            {
                default:
                    return this;
            }
        }
    }
}
