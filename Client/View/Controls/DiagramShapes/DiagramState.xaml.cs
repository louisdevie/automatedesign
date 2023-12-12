using AutomateDesign.Client.ViewModel.Documents;
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
        private StateViewModel viewModel;

        public StateViewModel ViewModel => this.viewModel;

        public override Shape MainShape => this.mainShape;

        public Position Position {
            get => this.viewModel.Position;
            set
            {
                this.viewModel.Position = value;
                this.OnMovement();
            }
        }

        public DiagramState(StateViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.Presentation = this;

            DataContext = this.viewModel;
            InitializeComponent();

            this.OnMovement();
        }

        public override void OnMovement()
        {
            Canvas.SetTop(this, this.viewModel.Position.Top);
            Canvas.SetLeft(this, this.viewModel.Position.Left);
            foreach (var transition in this.viewModel.AttachedTransitions)
            {
                (transition.Presentation as DiagramTransition)?.OnMovement();
            }
        }
    }
}
