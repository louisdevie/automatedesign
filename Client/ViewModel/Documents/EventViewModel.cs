using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class EventViewModel
    {
        private Event model;

        public string DisplayName => model.DisplayName;

        public Event Model => this.model;

        public EventViewModel(Event model)
        {
            this.model = model;
        }
    }
}
