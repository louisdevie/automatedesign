using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel.Users;
using System.Windows;
using AutomateDesign.Client.View.Pages;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour ChangePasswordPopup.xaml
    /// </summary>
    public partial class ChangePasswordPopup : Window, INavigationContainer
    {
        private Navigator navigator;

        public ChangePasswordPopup(Session? session, ChangePasswordViewModel viewModel)
        {
            InitializeComponent();

            this.navigator = new(this, new ChangePasswordView(viewModel));
            this.navigator.Session = session;

            WindowPreferences.ApplySize(WindowPreferences.WindowSize.Small, this);
            WindowPreferences.ApplyResizeMode(WindowPreferences.ResizeMode.MinimizeOnly, this);
        }

        public Window ParentWindow => this;

        public void ApplyPreferences(WindowPreferences preferences)
        {
            preferences.ApplyTitleTo(this);
        }

        public void ChangeContent(INavigable value)
        {
            this.daFrame.Content = value;
        }
    }
}
