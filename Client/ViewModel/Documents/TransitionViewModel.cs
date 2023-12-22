using AutomateDesign.Core.Documents;
using System.Collections.Generic;
using System.Linq;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class TransitionViewModel : BaseViewModel
    {
        private Transition model;
        private StateViewModel startVM;
        private StateViewModel endVM;
        private EventViewModel triggeredByVM;
        private object? presentation;

        public StateViewModel Start
        {
            get => this.startVM;
            set
            {
                this.startVM?.DetachTransition(this);
                this.startVM = value;
                this.model.Start = this.startVM.Model;
                this.startVM.AttachTransition(this);
                this.NotifyPropertyChanged(nameof(Start));
            }
        }

        public StateViewModel End
        {
            get => this.endVM;
            set
            {
                this.endVM?.DetachTransition(this);
                this.endVM = value;
                this.model.End = this.endVM.Model;
                this.endVM.AttachTransition(this);
                this.NotifyPropertyChanged(nameof(End));
            }
        }

        public EventViewModel TriggeredBy => this.triggeredByVM;

        public object? Presentation { get => this.presentation; set => this.presentation = value; }

        public Transition Model => this.model;

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
        public TransitionViewModel(Transition model, ExistingDocumentViewModel parentDocument)
        {
            this.model = model;
            this.Start = parentDocument.GetViewModelOf(this.model.Start);
            this.End = parentDocument.GetViewModelOf(this.model.End);
            this.triggeredByVM = parentDocument.GetViewModelOf(this.model.TriggeredBy);
        }
#pragma warning restore CS8618
    }
}
