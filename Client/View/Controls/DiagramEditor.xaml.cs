using AutomateDesign.Client.View.Controls.DiagramShapes;
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

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Logique d'interaction pour DiagramEditor.xaml
    /// </summary>
    public partial class DiagramEditor : UserControl
    {
        protected bool isDragging;
        private Point? clickPosition;
        private TranslateTransform? originTT;

        public DiagramEditor()
        {
            this.isDragging = false;

            InitializeComponent();
        }

        public void AddShape(DiagramShape shape)
        {
            this.frontCanvas.Children.Add(shape);
            shape.MouseLeftButtonDown += this.CanvasMouseLeftButtonDown;
            shape.MouseLeftButtonUp += this.CanvasMouseLeftButtonUp;
            shape.MouseMove += this.CanvasMouseMove;
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DiagramShape draggableControl)
            {
                this.originTT = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                this.isDragging = true;
                this.clickPosition = e.GetPosition(this);
                draggableControl.CaptureMouse();
            }
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            if (sender is DiagramShape draggableControl)
            {
                draggableControl.ReleaseMouseCapture();
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is DiagramShape draggableControl
                && this.isDragging
                && this.originTT is not null
                && this.clickPosition is not null)
            {
                Point currentPosition = e.GetPosition(this);
                var transform = draggableControl.RenderTransform as TranslateTransform ?? new TranslateTransform();
                transform.X = this.originTT.X + (currentPosition.X - this.clickPosition.Value.X);
                transform.Y = this.originTT.Y + (currentPosition.Y - this.clickPosition.Value.Y);
                draggableControl.RenderTransform = new TranslateTransform(transform.X, transform.Y);
            }
        }
    }
}

