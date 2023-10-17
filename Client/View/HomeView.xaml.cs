using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomeView : NavigablePage
    {
        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.FullScreen,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            InitializeComponent();
        }
    }
}
