using AutomateDesign.Client.View.Navigation;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Navigator navigator;

        public MainWindow()
        {
            InitializeComponent();

            this.navigator = new(this, this.frame, new LoginView());
        }
    }
}
