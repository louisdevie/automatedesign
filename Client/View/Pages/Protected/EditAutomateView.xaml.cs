using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.Model.Logic.Editor.States;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Controls.DiagramShapes;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Core.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour EditAutomateView.xaml
    /// </summary>
    public partial class EditAutomateView : NavigablePage, IEditorUI
    {
        private EditorContext context;

        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.Large,
            WindowPreferences.ResizeMode.Resizeable
        );

        public EditAutomateView()
        {
            InitializeComponent();
            BurgerMenu.Visibility = Visibility.Collapsed;
            ProfilMenu.Visibility = Visibility.Collapsed;

            this.context = new(this, new Document());
            this.diagramEditor.OnShapeSelected += this.DiagramEditorOnShapeSelected;
        }

        private void DiagramEditorOnShapeSelected(DiagramShape selected)
        {
            switch (selected)
            {
                case DiagramState state:
                    this.context.HandleEvent(new EditorEvent.SelectState(state.Model));
                    break;
            }
        }

        private void BurgerToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.BurgerMenu.Visibility = this.BurgerMenu.Visibility switch
            {
                Visibility.Visible => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        private void CliclProfilButton(object sender, RoutedEventArgs e)
        {
            if (ProfilMenu.Visibility == Visibility.Visible)
            {
                ProfilMenu.Visibility = Visibility.Collapsed;
            }
            else
            {
                //this.emailLabel.Content = this.Navigator.Session.UserEmail.Split('@')[0];
                this.emailLabel.Content = "automate.design";
                ProfilMenu.Visibility = Visibility.Visible;
            }
        }

        private void LogOutButton(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePwdButton(object sender, RoutedEventArgs e)
        {

        }

        private void AddStateButtonClick(object sender, RoutedEventArgs e)
        {
            this.context.HandleEvent(new EditorEvent.CreateState());
        }

        private void AddTransitionButtonClick(object sender, RoutedEventArgs e)
        {
            this.context.HandleEvent(new EditorEvent.CreateTransition());
        }

        public bool AskNewStateName(out string name)
        {
            InputDialog popup = new("Nouvel état", "Entrez le nom du nouvel état :");
            popup.Owner = this.Navigator.Window;
            bool result = popup.ShowDialog() ?? false;

            name = popup.UserInput;
            return result;
        }

        public void ShowTransitionGhost(State startState)
        {
            
        }

        public bool ChooseEvent(out IEvent evt)
        {
            SuggestionInputDialog popup = new(
                "Nouvelle transition", "Entrez le nom de l'évènement déclencheur :",
                this.context.Document.Events.Select(e => e.Name)
            );
            popup.Owner = this.Navigator.Window;
            bool result = popup.ShowDialog() ?? false;

            string name = popup.UserInput;

            if (this.context.Document.Events.FirstOrDefault(e => e.Name == name) is EnumEvent found)
            {
                evt = found;
            }
            else
            {
                evt = this.context.Document.CreateEnumEvent(name);
            }

            return result;
        }

        public void OnCreateState(State state)
        {
            this.diagramEditor.AddShape(new DiagramState(state));
        }

        public void OnModeChange(bool selectionMode)
        {
            this.diagramEditor.SelectionMode = selectionMode;
        }

        public void OnStateChange(EditorState state)
        {
            this.status.Content = state.StatusMessage;
        }

        public void OnCreateTransition(Transition transition)
        {
            this.diagramEditor.AddShape(new DiagramTransition(transition));
        }
    }
}
