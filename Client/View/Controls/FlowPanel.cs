using System;
using System.Windows.Controls;
using System.Windows;

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Un panneau qui arrange ses enfants dans des colonnes de taille égale.
    /// Les colonnes ont une largeur minimum et le panel utilise le plus de colonnes possible.
    /// C'est l'équivalent d'un <see cref="WrapPanel"/>, mais qui utilise toute la largeur disponible.
    /// </summary>
    public class FlowPanel : Panel
    {
        /// <summary>
        /// La taille minimum des colonnes.
        /// </summary>
        public double MinimumColumnWidth
        {
            get { return (double)GetValue(MinimumColumnWidthProperty); }
            set { SetValue(MinimumColumnWidthProperty, value); }
        }

        /// <summary>
        /// La taille minimum des colonnes.
        /// </summary>
        public static readonly DependencyProperty MinimumColumnWidthProperty
            = DependencyProperty.Register(
                "MinimumColumnWidth",
                typeof(double),
                typeof(FlowPanel),
                new PropertyMetadata(defaultValue: 200d)
            );

        protected override Size MeasureOverride(Size availableSize)
        {
            Size finalSize = new Size { Width = Math.Max(availableSize.Width, this.MinimumColumnWidth) };

            // on essaie de rentrer un maximum de colonnes dans la largeur donnée
            int maximumColumns = (int)Math.Floor(finalSize.Width / this.MinimumColumnWidth);
            double availableColumnWidth = finalSize.Width / maximumColumns;

            double currentRowHeight = 0;
            int childrenInCurrentRow = 0;
            foreach (UIElement child in this.Children)
            {
                child.Measure(new Size(availableColumnWidth, availableSize.Height));
                if (childrenInCurrentRow < maximumColumns)
                {
                    // chaque rangée prends la taille du plus grand enfant
                    currentRowHeight = Math.Max(child.DesiredSize.Height, currentRowHeight);
                }
                else
                {
                    finalSize.Height += currentRowHeight;
                    currentRowHeight = child.DesiredSize.Height;
                    childrenInCurrentRow = 0;
                }
                childrenInCurrentRow++;
            }
            finalSize.Height += currentRowHeight;

            return finalSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            // on essaie de rentrer un maximum de colonnes dans la largeur donnée
            int columns = (int)Math.Floor(finalSize.Width / this.MinimumColumnWidth);
            double columnWidth = finalSize.Width / columns;

            double currentRowHeight = 0;
            double currentRowTop = 0;
            int childrenInCurrentRow = 0;
            foreach (UIElement child in Children)
            {
                if (childrenInCurrentRow >= columns)
                {
                    childrenInCurrentRow = 0;
                    currentRowTop += currentRowHeight;
                    currentRowHeight = 0;
                }

                child.Arrange(
                    new Rect(
                        columnWidth * childrenInCurrentRow, currentRowTop,
                        columnWidth, child.DesiredSize.Height
                    )
                );
                currentRowHeight = Math.Max(child.DesiredSize.Height, currentRowHeight);
                childrenInCurrentRow++;
            }

            return finalSize;
        }
    }
}
