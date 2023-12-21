using AutomateDesign.Core.Documents;
using System.Collections.Generic;

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

        public override bool Equals(object? obj)
        {
            return obj is EnumEvent model &&
                   EqualityComparer<IEvent>.Default.Equals(this.model, model);
        }
    }
}
