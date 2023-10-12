using System;
using System.Windows.Controls;

namespace AutomateDesign.Client.View.Helpers
{
    public class NavigablePage : Page, INavigable
    {
        private Navigator? navigator;

        protected Navigator Navigator => this.navigator ?? throw new InvalidOperationException("La page n'a jamais été utilisée.");

        public void UseNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }
    }
}
