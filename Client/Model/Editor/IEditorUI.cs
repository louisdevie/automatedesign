using AutomateDesign.Client.Model.Editor.States;
using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomateDesign.Client.Model.Editor
{
    public interface IEditorUI
    {
        #region Prompts

        bool AskNewStateName(out string name);

        bool ChooseEvent(out IEvent evt);

        void ShowTransitionGhost(State startState);

        #endregion

        #region Events

        void OnCreateState(State state);

        void OnCreateTransition(Transition transition);

        void OnModeChange(bool selectionMode);

        void OnStateChange(EditorState state);

        #endregion
    }
}
