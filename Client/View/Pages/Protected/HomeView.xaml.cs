﻿using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.Users;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomeView : NavigablePage
    {
        private SessionViewModel? sessionVM;
        private DocumentCollectionViewModel documentsVM;

        public string CurrentUserEmail => this.sessionVM?.UserEmail ?? "";

        public ObservableCollection<DocumentViewModel> Documents => this.documentsVM;

        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.Large,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            this.documentsVM = new();

            Task.Run(this.documentsVM.Reload);

            DataContext = this;
            InitializeComponent();
        }

        public override void OnNavigatedToThis(bool clearedHistory)
        {
            if (this.Navigator.Session is Session session)
            {
                this.sessionVM = new SessionViewModel(session);
            }
        }

        private void HaveFocusRecherche(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (this.TextBoxRecherche.Text == "Rechercher")
            {
                this.TextBoxRecherche.Text = "";
            }
        }

        private void LostFocusRecherche(object sender, RoutedEventArgs e)
        {
            if (this.TextBoxRecherche.Text == "")
            {
                this.TextBoxRecherche.Focus();
                this.TextBoxRecherche.Text = "Rechercher";
            }
        }

        private void NewDocumentClick(object sender, RoutedEventArgs e)
        {
            this.Navigator.Go(new EditAutomateView());
        }

        private void CliclProfilButton(object sender, RoutedEventArgs e)
        {
            if (ProfilMenu.Visibility == Visibility.Visible)
            {
                ProfilMenu.Visibility = Visibility.Collapsed;
            }
            else
            {
                //this.emailLabel.Content = this.Navigator.Session.UserEmail.Split('@')[0];
                this.emailLabel.Content = "automate.design";
                ProfilMenu.Visibility = Visibility.Visible;
            }
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePasswordPopup popup = new(this.Navigator.Session, this.sessionVM!.ChangePassword());
            popup.Owner = this.Navigator.Window;

            popup.ShowDialog();
        }

        private async void SignOut(object sender, RoutedEventArgs e)
        {
            await this.sessionVM!.SignOutAsync();
        }
    }
}