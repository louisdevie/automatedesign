using AutomateDesign.Client.Model.Network;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Core.Documents;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomeView : NavigablePage
    {
        private UsersClient users;
        private DocumentCrypte document;
        private List<DocumentCrypte> items;

        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.FullScreen,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            this.users = new UsersClient();

            InitializeComponent();
            items = new List<DocumentCrypte>
            {
                new DocumentCrypte()
            };

            AumateList.ItemsSource = items;

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
            if (this.TextBoxRecherche.Text == "")
            {
                this.TextBoxRecherche.Focus();
                this.TextBoxRecherche.Text = "Rechercher";
            }
        }

        private void InitializationAutomate()
        {

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
            ChangePasswordPopup popup = new(this.Navigator.Session!);
            popup.Owner = this.Navigator.Window;

            popup.ShowDialog();
        }

        private void SignOut(object sender, RoutedEventArgs e)
        {
            this.users.DisconnectAsync(this.Navigator.Session!.Token)
            .ContinueWith(task =>
            {
                this.Navigator.Session = null;
                this.Navigator.Go(new LoginView());
            },
            TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// Ouvre un menu déroulant des options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSettings(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            btn.ContextMenu.IsOpen = true;
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            // Afficher une boîte de dialogue de confirmation
            MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer cet automate ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Supprimer l'élément
                this.users.DeleteAutomateAsync(document.IdDoc);
            }
        }


        private void EditClick(object sender, RoutedEventArgs e)
        {
            // Logique de modification ici
        }


    }
}
