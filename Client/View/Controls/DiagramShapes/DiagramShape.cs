using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View.Controls.DiagramShapes
{
    public abstract class DiagramShape : UserControl
    {
        public abstract Shape MainShape { get; }

        public void ChangeMode(bool selectionModeEnabled)
        {
            if (selectionModeEnabled)
            {
                this.MainShape.Cursor = Cursors.Hand;
            }
            else
            {
                this.MainShape.Cursor = Cursors.SizeAll;
            }
        }
    }
}
