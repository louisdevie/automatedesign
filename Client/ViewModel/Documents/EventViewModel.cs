using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.ViewModel.Documents
{
    public class EventViewModel
    {
        private IEvent model;

        public string DisplayName => model.DisplayName;

        public IEvent Model => this.model;

        public EventViewModel(IEvent model)
        {
            this.model = model;
        }
    }
}
