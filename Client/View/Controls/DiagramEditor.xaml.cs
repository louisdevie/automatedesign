using AutomateDesign.Client.View.Controls.DiagramShapes;
using AutomateDesign.Core.Documents;
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
        private bool selectionMode;

        public bool SelectionMode
        {
            get => this.selectionMode;
            set
            {
                this.selectionMode = value;
                foreach (var child in this.frontCanvas.Children) (child as DiagramShape)?.ChangeMode(selectionMode);
            }
        }

        public delegate void SelectedShapeEventHandler(DiagramShape selected);
        public event SelectedShapeEventHandler? OnShapeSelected;

        public DiagramEditor()
        {
            this.isDragging = false;

            InitializeComponent();
            this.SelectionMode = false;
        }

        public void AddShape(DiagramShape shape)
        {
            this.frontCanvas.Children.Add(shape);
            shape.ChangeMode(this.selectionMode);
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
                if (this.selectionMode)
                {
                    this.clickedOn = shape;
                }
                else
                {
                    this.originTT = shape.RenderTransform as TranslateTransform ?? new TranslateTransform();
                    this.isDragging = true;
                    this.clickPosition = e.GetPosition(this);
                    shape.CaptureMouse();
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
                if (this.selectionMode)
                {
                    if (shape == this.clickedOn)
                    {
                        this.OnShapeSelected?.Invoke(shape);
                    }
                    this.clickedOn = null;
                }
                else
                {
                    shape.ReleaseMouseCapture();
                }
            }
        }

        /// <summary>
        /// Gère les mouvements de souris sur les éléments du diagramme.
        /// </summary>
        private void ShapeMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.selectionMode
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
    }
}

