using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.Model.Logic.Editor.States;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Controls.DiagramShapes;
using AutomateDesign.Client.View.Helpers;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.Users;
using AutomateDesign.Core.Documents;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour EditAutomateView.xaml
    /// </summary>
    public partial class EditAutomateView : NavigablePage, IEditorUI
    {
        private EditorContext context;
        private ExistingDocumentViewModel viewModel;
        private SessionViewModel? sessionVM;

        public ExistingDocumentViewModel Document => this.viewModel;

        public DocumentHeaderViewModel Header => this.viewModel.Header;

        public Observable<string> StatusMessage { get; } = new("");

        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.Large,
            WindowPreferences.ResizeMode.Resizeable
        );

        public EditAutomateView(ExistingDocumentViewModel viewModel, SessionViewModel sessionVM)
        {
            this.viewModel = viewModel;
            DataContext = this;
            this.sessionVM = sessionVM;

            this.context = new(this.viewModel.Document, this);
            this.context.EditorStateChanged += this.OnEditorStateChanged;
            this.context.AddModificationObserver(this.viewModel);

            InitializeComponent();
            BurgerMenu.Visibility = Visibility.Collapsed;
            ProfilMenu.Visibility = Visibility.Collapsed;

            this.diagramEditor.ViewModel = this.viewModel;
            this.diagramEditor.OnShapeSelected += this.DiagramEditorOnShapeSelected;
            this.diagramEditor.OnStatePlaced += this.DiagramEditorOnStatePlaced;

            this.context.Initialize();
        }

        private void OnEditorStateChanged(EditorState state)
        {
            this.StatusMessage.Value = state.StatusMessage;
            this.diagramEditor.Mode = this.context.Mode;
        }

        #region Évènements du diagramme

        private void DiagramEditorOnShapeSelected(DiagramShape selected)
        {
            switch (selected)
            {
                case DiagramState state:
                    this.context.HandleEvent(new EditorEvent.SelectState(state.ViewModel.Model));
                    break;
            }
        }

        private void DiagramEditorOnStatePlaced(Position position)
        {
            this.context.HandleEvent(new EditorEvent.FinishCreatingState(position));
        }

        #endregion

        private void BurgerToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.BurgerMenu.Visibility = this.BurgerMenu.Visibility switch
            {
                Visibility.Visible => Visibility.Collapsed,
                _ => Visibility.Visible
            };
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            ProgressDialog popup = new(
                title: "Enregistrement",
                progressMessage: "Enregistrement du document...",
                successMessage: $"« {this.viewModel.Name} » a bien été enregistré."
            );

            Task.Run(() => this.viewModel.Save(popup));

            if (popup.ShowDialog() == true)
            {
                this.viewModel.Unload();
                this.Navigator.Back();
            }
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

        private async void LogOutButton(object sender, RoutedEventArgs e)
        {
            await this.sessionVM!.SignOutAsync();
            this.Navigator.Go(new SignInView(),true);
        }

        private void ChangePwdButton(object sender, RoutedEventArgs e)
        {

        }

        private void AddStateButtonClick(object sender, RoutedEventArgs e)
        {
            this.context.HandleEvent(new EditorEvent.BeginCreatingState());
        }

        private void AddTransitionButtonClick(object sender, RoutedEventArgs e)
        {
            this.context.HandleEvent(new EditorEvent.BeginCreatingTransition());
        }

        private void PageKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.context.HandleEvent(new EditorEvent.Cancel());
            }
        }

        private void ImportClick(object sender, RoutedEventArgs e)
        {
            var importDialog = new ReverseEngineeringWindow(this.viewModel);
            importDialog.ShowDialog();
        }

        #region Implémentation de IEditorUI

        public bool PromptForStateName([NotNullWhen(true)] out string? name)
        {
            InputDialog popup = new("Nouvel état", "Entrez le nom du nouvel état :");
            popup.Owner = this.Navigator.ParentWindow;
            bool result = popup.ShowDialog() ?? false;

            name = popup.UserInput;
            return result;
        }

        public bool PromptForEvent([NotNullWhen(true)] out IEvent? evt)
        {
            SuggestionInputDialog popup = new(
                "Nouvelle transition", "Entrez le nom de l'évènement déclencheur :",
                this.context.Document.Events.Select(e => e.Name)
            );
            popup.Owner = this.Navigator.ParentWindow;
            bool result = popup.ShowDialog() ?? false;

            string name = popup.UserInput;

            if (this.context.Document.Events.FirstOrDefault(e => e.Name == name) is EnumEvent found)
            {
                evt = found;
            }
            else
            {
                evt = this.context.AddEnumEvent(name);
            }

            return result;
        }

        public void ShowStateToAdd() => this.diagramEditor.ShowStateGhost();

        #endregion
    }
}
