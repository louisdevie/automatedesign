﻿using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Logic.Editor.States
{
    /// <summary>
    /// On attends que l'état de fin soit choisi pour ajouter une transition.
    /// </summary>
    internal class NewTransitionSelectEndState : EditorState
    {
        private State firstState;

        public NewTransitionSelectEndState(State firstState)
        {
            this.firstState = firstState;
        }

        public override string StatusMessage => "Sélectionnez un état d'arrivée";

        public override void Action(EditorEvent e, EditorContext ctx)
        {
            switch (e)
            {
                case EditorEvent.SelectState selectState:
                    ctx.AddTransition(this.firstState, selectState.state);
                    ctx.Mode = EditorMode.Move;
                    break;
            }
        }

        public override EditorState Next(EditorEvent e)
        {
            EditorState nextState = this;

            switch (e)
            {
                case EditorEvent.SelectState selectState:
                    nextState = new ReadyState();
                    break;
            }

            return nextState;
        }
    }
}