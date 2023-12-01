using AutomateDesign.Core.Documents;
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

namespace AutomateDesign.Client.View.Controls.DiagramShapes
{
    /// <summary>
    /// Un état sur le diagramme.
    /// </summary>
    public partial class DiagramState : DiagramShape
    {
        private State model;
        private HashSet<DiagramTransition> attachedTransitions;

        public State Model => this.model;

        public IEnumerable<DiagramTransition> AttachedTransitions => this.attachedTransitions;

        public override Shape MainShape => this.mainShape;

        public DiagramState(State model)
        {
            this.model = model;
            this.attachedTransitions = new();

            DataContext = this.model;
            InitializeComponent();
        }

        public void AttachTransition(DiagramTransition transition) => this.attachedTransitions.Add(transition);

        public void DetachTransition(DiagramTransition transition) => this.attachedTransitions.Remove(transition);

        public override void OnMovement()
        {
            foreach (var transition in this.AttachedTransitions)
            {
                transition.OnMovement();
            }
        }
    }
}
