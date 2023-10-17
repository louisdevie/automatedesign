using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour EditAutomateView.xaml
    /// </summary>
    public partial class EditAutomateView : NavigablePage
    {
        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.FullScreen,
            WindowPreferences.ResizeMode.Resizeable
        );

        public EditAutomateView()
        {
            InitializeComponent();
            BurgerMenu.Visibility = Visibility.Collapsed;

        }

        private void BurgerToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (BurgerMenu.Visibility == Visibility.Visible)
            {
                BurgerMenu.Visibility = Visibility.Collapsed;
            }
            else
            {
                BurgerMenu.Visibility = Visibility.Visible;
            }
        }
    }
}
