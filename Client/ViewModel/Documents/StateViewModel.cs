using AutomateDesign.Core.Documents;
using System.Collections.Generic;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class StateViewModel : BaseViewModel
    {
        private State model;
        private object? presentation;
        private List<TransitionViewModel> attachedTransitionVMs;

        public string Name
        {
            get => this.model.Name;
            set { this.model.Name = value; this.NotifyPropertyChanged(); }
        }

        public Position Position
        {
            get => this.model.Position;
            set { this.model.Position = value; this.NotifyPropertyChanged(); }
        }

        public object? Presentation { get => this.presentation; set => this.presentation = value; }

        public IEnumerable<TransitionViewModel> AttachedTransitions => this.attachedTransitionVMs;

        public State Model => this.model;

        public StateViewModel(State model)
        {
            this.model = model;
            this.attachedTransitionVMs = new List<TransitionViewModel>();
        }

        public void DetachTransition(TransitionViewModel transition)
        {
            this.attachedTransitionVMs.Remove(transition);
        }

        public void AttachTransition(TransitionViewModel transition)
        {
            this.attachedTransitionVMs.Add(transition);
        }

        public override bool Equals(object? obj)
        {
            return obj is State model &&
                   EqualityComparer<State>.Default.Equals(this.model, model);
        }
    }
}