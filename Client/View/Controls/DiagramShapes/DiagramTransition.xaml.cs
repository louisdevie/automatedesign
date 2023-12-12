using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Core.Documents;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View.Controls.DiagramShapes
{
    /// <summary>
    /// Une transition du diagramme.
    /// </summary>
    public partial class DiagramTransition : DiagramShape
    {
        private TransitionViewModel viewModel;
        private DiagramState? start, end;

        public override Shape MainShape => this.line;

        public DiagramTransition(TransitionViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.Presentation = this;

            DataContext = this.viewModel;
            InitializeComponent();
        }

        public void Reattach()
        {
            start = this.viewModel.Start.Presentation as DiagramState;
            end = this.viewModel.End.Presentation as DiagramState;
            this.Redraw();
        }

        private Size GetLabelSize()
        {
            Size size = new(this.eventLabel.ActualWidth, this.eventLabel.ActualHeight);

            if (size.Width == 0 && size.Height == 0)
            {
                this.eventLabel.Measure(new Size(100, 100));
                size = this.eventLabel.DesiredSize;
            }

            return size;
        }

        private void Redraw()
        {
            bool lineRedrawn = false;
            if (this.start is not null)
            {
                Vector2 startPosition = this.start.Position.Center;
                this.line.X1 = startPosition.X;
                this.line.Y1 = startPosition.Y;
                lineRedrawn = true;
            }

            if (this.end is not null)
            {
                Vector2 endPosition = this.end.Position.Center;
                this.line.X2 = endPosition.X;
                this.line.Y2 = endPosition.Y;
                lineRedrawn = true;
            }

            if (lineRedrawn)
            {
                Size labelSize = this.GetLabelSize();
                this.eventLabel.RenderTransform = new TranslateTransform(
                    (this.line.X1 + this.line.X2 - labelSize.Width) / 2,
                    (this.line.Y1 + this.line.Y2 - labelSize.Height) / 2
                );
            }
        }

        public override void OnMovement()
        {
            this.Redraw();
        }
    }
}
