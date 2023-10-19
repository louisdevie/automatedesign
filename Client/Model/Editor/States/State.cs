using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Editor.States
{
    public abstract class State
    {
        public abstract string Description { get; }

        public abstract State Next(Event e);

        public abstract void Action(Event e, EditorContext ctx);
    }
}
