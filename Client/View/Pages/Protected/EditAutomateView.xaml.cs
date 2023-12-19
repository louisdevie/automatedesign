using AutomateDesign.Client.Model;
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
using Microsoft.Win32;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        /// <summary>
        /// Ouvre une boîte de dialogue de sauvegarde pour exporter une image dans un format spécifié
        /// </summary>
        /// <param name="format">Le format de l'image (PNG, JPEG)</param>
        /// <param name="extension">L'extension de fichier correspondant au format d'image (png, jpg)</param>
        /// <param name="captureAndSaveMethod">La méthode qui capture et enregistre l'image, prenant un RenderTargetBitmap et un chemin de fichier en paramètres</param>
        private void ExportImage(string format, string extension, Action<RenderTargetBitmap, string> captureAndSaveMethod)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Sélectionnez un dossier";
            saveFileDialog.Filter = $"Image {format} (*.{extension})|*.{extension}";
            saveFileDialog.FileName = this.viewModel.Name;

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                if (!filePath.EndsWith($".{extension}", StringComparison.OrdinalIgnoreCase))
                {
                    filePath += $".{extension}";
                }

                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)diagramEditor.ActualWidth, (int)diagramEditor.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                captureAndSaveMethod(renderTargetBitmap, filePath);
            }
        }

        private void ExportPng(object sender, RoutedEventArgs e)
        {
            ExportImage("PNG", "png", PngCaptureDiagramEditor);
        }

        private void ExportJpg(object sender, RoutedEventArgs e)
        {
            ExportImage("JPEG", "jpg", JpgSaveDiagramEditor);
        }

        private void ExportLatex(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Génère l'image sous format png
        /// </summary>
        /// <param name="renderTargetBitmap">tableau de pixel</param>
        /// <returns></returns>
        private PngBitmapEncoder GeneratePngDiagramEditor(RenderTargetBitmap renderTargetBitmap)
        {
            renderTargetBitmap.Render(diagramEditor.FrontCanvas);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            return pngImage;
        }

        /// <summary>
        /// Récupère et sauvegarde une image sous format png
        /// </summary>
        /// <param name="renderTargetBitmap">tableau de pixel</param>
        /// <param name="filePath">chemin de sauvegarde</param>
        private void PngCaptureDiagramEditor(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            PngBitmapEncoder pngImage = GeneratePngDiagramEditor(renderTargetBitmap);
            using (var stream = System.IO.File.Create(filePath))
            {
                pngImage.Save(stream);
            }
        }

        /// <summary>
        /// Génère l'image sous format jpg
        /// </summary>
        /// <param name="renderTargetBitmap">tableau de pixel</param>
        /// <returns></returns>
        private JpegBitmapEncoder GenerateJpgDiagramEditor(RenderTargetBitmap renderTargetBitmap)
        {
            renderTargetBitmap.Render(diagramEditor);

            JpegBitmapEncoder jpegImage = new JpegBitmapEncoder();
            jpegImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            return jpegImage;
        }

        /// <summary>
        /// Récupère et sauvegarde une image sous format jpg
        /// </summary>
        /// <param name="renderTargetBitmap">tableau de pixel</param>
        /// <param name="filePath">chemin de sauvegarde</param>
        private void JpgSaveDiagramEditor(RenderTargetBitmap renderTargetBitmap, string filePath)
        {
            JpegBitmapEncoder jpegImage = GenerateJpgDiagramEditor(renderTargetBitmap);
            using (var stream = System.IO.File.Create(filePath))
            {
                jpegImage.Save(stream);
            }
        }

        private void PageKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.context.HandleEvent(new EditorEvent.Cancel());
            }
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
