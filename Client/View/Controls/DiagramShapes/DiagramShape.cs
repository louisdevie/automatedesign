using AutomateDesign.Client.Model.Logic.Editor;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View.Controls.DiagramShapes
{
    /// <summary>
    /// Un élément du diagramme.
    /// </summary>
    public abstract class DiagramShape : UserControl
    {
        /// <summary>
        /// La forme principale qui définit l'élément.
        /// </summary>
        public abstract Shape MainShape { get; }

        /// <summary>
        /// Réagis au changement de mode de l'éditeur. Par défaut, adapte le curseur de la souris.
        /// </summary>
        /// <param name="mode">Le nouveau mode de l'éditeur.</param>
        public virtual void ChangeMode(EditorMode mode)
        {
            switch(mode)
            {
                case EditorMode.Move:
                    this.MainShape.Cursor = Cursors.SizeAll;
                    break;

                case EditorMode.Select:
                    this.MainShape.Cursor = Cursors.Hand;
                    break;

                case EditorMode.Place:
                    this.MainShape.Cursor = Cursors.No;
                    break;
            }
        }

        /// <summary>
        /// Réagis au mouvement de l'élément.
        /// </summary>
        public abstract void OnMovement();
    }
}
