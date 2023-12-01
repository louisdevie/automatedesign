using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.View.Controls.DiagramShapes;
using AutomateDesign.Core.Documents;
using System;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private TranslateTransform? originTT;
        private DiagramShape? clickedOn;
        private DiagramStateGhost? stateGhost;
        private EditorMode mode;

        public EditorMode Mode
        {
            get => this.mode;
            set
            {
                this.mode = value;
                foreach (var child in this.frontCanvas.Children) (child as DiagramShape)?.ChangeMode(mode);
            }
        }

        public delegate void SelectedShapeEventHandler(DiagramShape selected);
        public event SelectedShapeEventHandler? OnShapeSelected;

        public delegate void StatePlacedEventHandler(Point position);
        public event StatePlacedEventHandler? OnStatePlaced;

        public DiagramEditor()
        {
            this.isDragging = false;

            InitializeComponent();
            this.Mode = EditorMode.Move;
            this.frontCanvas.MouseLeftButtonDown += this.CanvasMouseLeftButtonDown;
            this.frontCanvas.MouseLeftButtonUp += this.CanvasMouseLeftButtonUp;
            this.frontCanvas.MouseMove += this.CanvasMouseMove;
        }

        public void AddShape(DiagramShape shape)
        {
            this.frontCanvas.Children.Add(shape);
            shape.ChangeMode(this.mode);
            shape.MouseLeftButtonDown += this.ShapeMouseLeftButtonDown;
            shape.MouseLeftButtonUp += this.ShapeMouseLeftButtonUp;
            shape.MouseMove += this.ShapeMouseMove;
        }

        public void AddShape(DiagramTransition transition)
        {
            this.AddShape((DiagramShape)transition);
            Panel.SetZIndex(transition, -1);
            transition.Reattach(this);
        }

        public DiagramState? FindShapeFor(State state)
        {
            foreach (var child in this.frontCanvas.Children)
            {
                if (child is DiagramState diagramState && diagramState.Model == state)
                {
                    return diagramState;
                }
            }
            return null;
        }

        /// <summary>
        /// Gère l'enfoncement du bouton gauche sur les éléments du diagramme.
        /// </summary>
        private void ShapeMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DiagramShape shape)
            {
                switch (this.mode)
                {
                    case EditorMode.Move:
                        this.originTT = shape.RenderTransform as TranslateTransform ?? new TranslateTransform();
                        this.isDragging = true;
                        this.clickPosition = e.GetPosition(this);
                        shape.CaptureMouse();
                        break;

                    case EditorMode.Select:
                        this.clickedOn = shape;
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
                && this.originTT is not null
                && this.clickPosition is not null)
            {
                Point currentPosition = e.GetPosition(this);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = this.originTT.X + (currentPosition.X - this.clickPosition.Value.X);
                transform.Y = this.originTT.Y + (currentPosition.Y - this.clickPosition.Value.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
                draggableControl.OnMovement();               
            }
        }

        /// <summary>
        /// Ahoute un fantôme d'état qui suit la souris.
        /// </summary>
        public void AddStateGhost()
        {
            this.stateGhost = new DiagramStateGhost();
            this.frontCanvas.Children.Add(this.stateGhost);
            Panel.SetZIndex(this.stateGhost, 1000);
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (this.mode == EditorMode.Place && this.stateGhost is DiagramStateGhost ghost)
            {
                Point mousePositionOnCanvas = e.GetPosition(this.frontCanvas);
                Canvas.SetLeft(ghost, mousePositionOnCanvas.X - (ghost.ActualWidth / 2));
                Canvas.SetTop(ghost, mousePositionOnCanvas.Y - (ghost.ActualHeight / 2));
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.mode == EditorMode.Place && this.stateGhost is DiagramStateGhost ghost)
            {
                Point mousePositionOnCanvas = e.GetPosition(this.frontCanvas);
                this.OnStatePlaced?.Invoke(mousePositionOnCanvas);
            }
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}

