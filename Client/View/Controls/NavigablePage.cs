using System;
using System.Windows.Controls;
using AutomateDesign.Client.View.Navigation;

namespace AutomateDesign.Client.View.Controls
{
    public class NavigablePage : Page, INavigable
    {
        private Navigator? navigator;

        protected Navigator Navigator => this.navigator ?? throw new InvalidOperationException("La page n'a jamais été affichée.");

        public void UseNavigator(Navigator navigator)
        {
            this.navigator = navigator;
        }

        public virtual WindowPreferences Preferences => new();

        public virtual void OnNavigatedToThis(bool clearedHistory) { }

        public virtual void OnWentBackToThis() { }
    }
}
