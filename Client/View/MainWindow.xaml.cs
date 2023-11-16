using AutomateDesign.Client.DependencyInjection;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Navigation;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INavigationContainer
    {
        private Navigator navigator;

        public MainWindow()
        {
            DependencyContainer.Current.RegisterSingleton<IUsersClient>(new UsersClient());

            InitializeComponent();

            this.navigator = new(this, new LoginView());
        }

        public Window Window => this;

        public void ApplyPreferences(WindowPreferences preferences)
        {
            preferences.ApplySize(this);
            preferences.ApplyResizeMode(this);
            preferences.ApplyTitleTo(this);
        }

        public void ChangeContent(INavigable value)
        {
            this.frame.Content = value;
        }
    }
}
