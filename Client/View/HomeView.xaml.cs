using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using System.Collections.Generic;
using System.ComponentModel;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomeView : NavigablePage
    {
        private List<string> items;
        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.FullScreen,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            items = new List<string>();

            InitializeComponent();
        }

        private void HaveFocusRecherche(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (this.TextBoxRecherche.Text == "Rechercher") 
            {
                this.TextBoxRecherche.Text = "";
            }
        }

        private void LostFocusRecherche(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.TextBoxRecherche.Text == "") {
                this.TextBoxRecherche.Focus();
                this.TextBoxRecherche.Text = "Rechercher";
            }
        }



    }
}
