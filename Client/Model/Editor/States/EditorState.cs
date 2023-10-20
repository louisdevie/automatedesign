using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Editor.States
{
    public abstract class EditorState
    {
        public abstract string Description { get; }

        public abstract EditorState Next(EditorEvent e);

        public abstract void Action(EditorEvent e, EditorContext ctx);
    }
}
