using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.View.Controls.DiagramShapes;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Core.Documents;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Un canvas sur lequel ont peut placer des éléments de diagramme.
    /// </summary>
    public partial class DiagramEditor : UserControl
    {
        protected bool isDragging;
        private Point? clickPosition;
        private Position? originalPosition;
        private DiagramShape? clickedOn;
        private ExistingDocumentViewModel? viewmodel;
        private EditorMode mode;

        public EditorMode Mode
        {
            get => this.mode;
            set
            {
                if (this.mode == EditorMode.Place && this.StateGhostAvailable) this.HideStateGhost();

                this.mode = value;
                
                foreach (var child in this.frontCanvas.Children) (child as DiagramShape)?.ChangeMode(mode);
            }
        }

        public ExistingDocumentViewModel ViewModel
        {
            set
            {
                this.viewmodel = value;

                foreach (StateViewModel state in this.viewmodel.States) this.AddShape(new DiagramState(state));
                foreach (TransitionViewModel transition in this.viewmodel.Transitions) this.AddTransition(new DiagramTransition(transition));

                this.viewmodel.States.CollectionChanged += this.ElementsChanged;
                this.viewmodel.Transitions.CollectionChanged += this.ElementsChanged;
            }
        }

        private void ElementsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null) foreach (object? added in e.NewItems) ElementAdded(added);
                    break;
            }
        }

        private void ElementAdded(object? added)
        {
            switch (added)
            {
                case StateViewModel stateViewModel:
                    this.AddShape(new DiagramState(stateViewModel));
                    break;

                case TransitionViewModel transitionViewModel:
                    this.AddTransition(new DiagramTransition(transitionViewModel));
                    break;
            }
        }

        private bool StateGhostAvailable => this.mode == EditorMode.Place
                                         && this.stateGhost.Visibility == Visibility.Visible;

        public delegate void SelectedShapeEventHandler(DiagramShape selected);
        public event SelectedShapeEventHandler? OnShapeSelected;

        public delegate void StatePlacedEventHandler(Position position);
        public event StatePlacedEventHandler? OnStatePlaced;

        public DiagramEditor()
        {
            this.isDragging = false;

            InitializeComponent();
            this.HideStateGhost();
            this.Mode = EditorMode.Move;
            this.frontCanvas.MouseLeftButtonDown += this.CanvasMouseLeftButtonDown;
            this.frontCanvas.MouseLeftButtonUp += this.CanvasMouseLeftButtonUp;
            this.frontCanvas.MouseMove += this.CanvasMouseMove;
        }

        /// <summary>
        /// Ajoute un élément quelconque au diagramme. Il vaut mieux utiliser <see cref="AddTransition(DiagramTransition)"/>
        /// pour les transitions.
        /// </summary>
        /// <param name="shape">L'élément à ajouter.</param>
        private void AddShape(DiagramShape shape)
        {
            this.frontCanvas.Children.Add(shape);
            shape.ChangeMode(this.mode);
            shape.MouseLeftButtonDown += this.ShapeMouseLeftButtonDown;
            shape.MouseLeftButtonUp += this.ShapeMouseLeftButtonUp;
            shape.MouseMove += this.ShapeMouseMove;
        }

        /// <summary>
        /// Ajoute une transition au diagramme.
        /// </summary>
        /// <param name="transition">La transition à rajouter.</param>
        public void AddTransition(DiagramTransition transition)
        {
            this.AddShape(transition);
            Panel.SetZIndex(transition, -1);
            transition.Reattach();
        }

        /// <summary>
        /// Gère l'enfoncement du bouton gauche sur les éléments du diagramme.
        /// </summary>
        private void ShapeMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DiagramState state)
            {
                switch (this.mode)
                {
                    case EditorMode.Move:
                        this.originalPosition = state.Position;
                        this.isDragging = true;
                        this.clickPosition = e.GetPosition(this);
                        state.CaptureMouse();
                        break;

                    case EditorMode.Select:
                        this.clickedOn = state;
                        break;
                }
            }
        }

        /// <summary>
        /// Gère le relâchement du bouton gauche sur les éléments du diagramme.
        /// </summary>
        private void ShapeMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            if (sender is DiagramShape shape)
            {
                switch (this.mode)
                {
                    case EditorMode.Move:
                        shape.ReleaseMouseCapture();
                        break;

                    case EditorMode.Select:
                        if (shape == this.clickedOn)
                        {
                            this.OnShapeSelected?.Invoke(shape);
                        }
                        this.clickedOn = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Gère les mouvements de souris sur les éléments du diagramme.
        /// </summary>
        private void ShapeMouseMove(object sender, MouseEventArgs e)
        {
            if (this.mode == EditorMode.Move
                && sender is DiagramState draggableControl
                && this.isDragging
                && this.originalPosition is not null
                && this.clickPosition is not null)
            {
                Point currentPosition = e.GetPosition(this);
                draggableControl.Position = this.originalPosition.Value.MoveBy(
                    (float)(currentPosition.X - this.clickPosition.Value.X),
                    (float)(currentPosition.Y - this.clickPosition.Value.Y)
                );
            }
        }

        /// <summary>
        /// Affiche un fantôme d'état qui suit la souris.
        /// </summary>
        public void ShowStateGhost()
        {
            this.stateGhost.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Cache le fantôme d'état affiché précedemment avec <see cref="ShowStateGhost()"/>
        /// </summary>
        public void HideStateGhost()
        {
            this.stateGhost.Visibility = Visibility.Collapsed;
            Canvas.SetTop(this.stateGhost, -this.stateGhost.Height * 2);
        }

        /// <summary>
        /// Gère les mouvements de souris sur la zone de diagramme.
        /// </summary>
        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (this.StateGhostAvailable)
            {
                Point mousePositionOnCanvas = e.GetPosition(this.frontCanvas);
                Canvas.SetLeft(this.stateGhost, mousePositionOnCanvas.X - (this.stateGhost.ActualWidth / 2));
                Canvas.SetTop(this.stateGhost, mousePositionOnCanvas.Y - (this.stateGhost.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Gère le relâchement du bouton gauche de la souris sur la zone de diagramme.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.StateGhostAvailable)
            {
                Point mousePositionOnCanvas = e.GetPosition(this.frontCanvas);
                this.OnStatePlaced?.Invoke(
                    new Position(
                        (float)(mousePositionOnCanvas.X - (this.stateGhost.Width / 2)),
                        (float)(mousePositionOnCanvas.Y - (this.stateGhost.Height / 2)),
                        (float)this.stateGhost.Width,
                        (float)this.stateGhost.Height
                    )
                );
            }
        }

        /// <summary>
        /// Gère l'enfoncement du bouton gauche de la souris sur la zone de diagramme.
        /// </summary>
        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Enregistre l'automate sous format png
        /// </summary>
        /// <returns>Image Png</returns>
        public void PngCaptureDiagramEditor(string filePath)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(this);

            PngBitmapEncoder pngImage = new PngBitmapEncoder();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            using (var stream = System.IO.File.Create(filePath))
            {
                pngImage.Save(stream);
            }           
        }
    }
}

