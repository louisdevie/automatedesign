using System;

namespace AutomateDesign.Client.Model.Editor.States
{
    public class ReadyState : State
    {
        public override string Description => "Prêt";

        public override void Action(Event e, EditorContext ctx)
        {
            switch (e) {
                case Event.AddState:
                    ctx.AddState();
                    break;
            }
        }

        public override State Next(Event e)
        {
            switch (e)
            {
                default:
                    return this;
            }
        }
    }
}
