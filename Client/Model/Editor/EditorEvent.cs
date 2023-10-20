using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Editor
{
    public abstract record EditorEvent
    {
        public record CreateState() : EditorEvent;

        public record CreateTransition() : EditorEvent;

        public record SelectState(State state): EditorEvent;

    }
}
