using AutomateDesign.Core.Documents;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View.Controls.DiagramShapes
{
    /// <summary>
    /// Logique d'interaction pour DiagramTransition.xaml
    /// </summary>
    public partial class DiagramTransition : DiagramShape
    {
        private Transition model;
        private DiagramState? start, end;

        public override Shape MainShape => this.line;

        public DiagramTransition(Transition model)
        {
            this.model = model;

            DataContext = this.model;
            InitializeComponent();
        }

        public void Reattach(DiagramEditor parentEditor)
        {
            this.start?.DetachTransition(this);
            this.end?.DetachTransition(this);

            start = parentEditor.FindShapeFor(this.model.Start);
            end = parentEditor.FindShapeFor(this.model.End);

            this.start?.AttachTransition(this);
            this.end?.AttachTransition(this);

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

        public void Redraw()
        {
            if (this.start is not null && this.end is not null)
            {
                this.line.X1 = (this.start.ActualWidth/ 2) + (this.start.RenderTransform as TranslateTransform)?.X ?? 50;
                this.line.Y1 = (this.start.ActualHeight / 2) + (this.start.RenderTransform as TranslateTransform)?.Y ?? 50;
                this.line.X2 = (this.end.ActualWidth / 2) + (this.end.RenderTransform as TranslateTransform)?.X ?? 50;
                this.line.Y2 = (this.end.ActualHeight / 2) + (this.end.RenderTransform as TranslateTransform)?.Y ?? 50;

                Size labelSize = this.GetLabelSize();
                this.eventLabel.RenderTransform = new TranslateTransform(
                    (this.line.X1 + this.line.X2 - labelSize.Width) /2,
                    (this.line.Y1 + this.line.Y2 - labelSize.Height) /2
                );
            }
        }
    }
}
