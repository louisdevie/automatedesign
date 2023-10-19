using AutomateDesign.Client.Model.Editor.States;

namespace AutomateDesign.Client.Model.Editor
{
    public class EditorContext
    {
        private State state;

        public EditorContext() { 
            this.state = new ReadyState();
        }
    }
}
