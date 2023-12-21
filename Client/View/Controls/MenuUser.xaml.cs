using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.View.Pages;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.Users;
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

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Logique d'interaction pour MenuUser.xaml
    /// </summary>
    public partial class MenuUser : UserControl
    {
        private SessionViewModel? sessionVM;
        private Navigator? navigator;

        public SessionViewModel? SessionVM { set { this.sessionVM = value; } }

        public Navigator? Navigator { set { this.navigator = value; } }

        public MenuUser()
        {
            InitializeComponent();
        }

        private void ProfilButton_Click(object sender, RoutedEventArgs e)
        {
            this.emailLabel.Header = this.sessionVM!.UserEmail.Split('@')[0];
            ProfilMenu.IsOpen = !ProfilMenu.IsOpen;
        }

        private async void LogOutButton(object sender, RoutedEventArgs e)
        {
            await this.sessionVM!.SignOutAsync();
            this.navigator!.Go(new SignInView(), true);
        }

        private void ChangePwdButton(object sender, RoutedEventArgs e)
        {
            ChangePasswordPopup popup = new(this.navigator!.Session, this.sessionVM!.ChangePassword());
            popup.Owner = this.navigator.ParentWindow;

            popup.ShowDialog();
        }
    }
}
