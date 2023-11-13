using AutomateDesign.Core.Documents;
using System;

namespace AutomateDesign.Client.Model.Editor.States
{
    public class ReadyState : EditorState
    {
        public override string Description => "Prêt";

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
                    return new NewTransitionSelectFirstState();
                
                default:
                    return this;
            }
        }
    }
}
