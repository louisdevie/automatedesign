using AutomateDesign.Client.Model.Editor.States;
using AutomateDesign.Core.Documents;
using System;

namespace AutomateDesign.Client.Model.Editor
{
    public class EditorContext
    {
        #region Attributes

        private EditorState state;
        private Document document;
        private IEditorUI editorUI;
        private bool selectionModeEnabled;

        #endregion

        #region Properties

        public Document Document => this.document;

        public bool SelectionModeEnabled => this.selectionModeEnabled;

        #endregion

        #region Constructors

        public EditorContext(IEditorUI editor, Document document)
        {
            this.state = new ReadyState();
            this.document = document;
            this.editorUI = editor;
            this.editorUI.OnStateChange(this.state);
        }

        #endregion

        #region Event handling

        public void HandleEvent(EditorEvent e)
        {
            this.state.Action(e, this);
            this.state = this.state.Next(e);
            this.editorUI.OnStateChange(this.state);
        }

        #endregion

        #region Actions

        public void CreateState()
        {
            if (this.editorUI.AskNewStateName(out string name))
            {
                State state = this.document.CreateState(name);
                this.editorUI.OnCreateState(state);
            }
        }

        internal void EnterSelectionMode()
        {
            this.selectionModeEnabled = true;
            this.editorUI.OnModeChange(this.selectionModeEnabled);
        }

        internal void ExitSelectionMode()
        {
            this.selectionModeEnabled = false;
            this.editorUI.OnModeChange(this.selectionModeEnabled);
        }

        internal void StartDrawingTransition(State startState)
        {
            this.editorUI.ShowTransitionGhost(startState);
        }

        internal void CreateTransition(State startState, State endState)
        {
            if (this.editorUI.ChooseEvent(out IEvent evt))
            {
                Transition transition = this.document.CreateTransition(startState, endState, evt);
                this.editorUI.OnCreateTransition(transition);
            }
        }

        #endregion
    }
}

