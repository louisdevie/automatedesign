using AutomateDesign.Client.DependencyInjection;
using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Navigation;
using System.Windows;
using AutomateDesign.Client.View.Pages;

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
            DependencyContainer.Current.RegisterSingleton<IDocumentsClient>(new DocumentsClient());

            InitializeComponent();

            this.navigator = new(this, new SignInView());
        }

        public Window ParentWindow => this;

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
