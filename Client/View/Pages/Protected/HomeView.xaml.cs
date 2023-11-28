using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.Users;
using System.Collections.ObjectModel;
using AutomateDesign.Core.Documents;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

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
            items = new List<Automate>
            {
                new Automate("auto1", "16/10/2023"),
                new Automate("auto2", "17/10/2023"),
                new Automate("auto3", "18/10/2023"),
                new Automate("auto4", "18/10/2023"),
                new Automate("auto5", "18/10/2023"),
                new Automate("auto6", "18/10/2023"),
                new Automate("auto7", "18/10/2023"),
                new Automate("auto8", "18/10/2023"),
                new Automate("auto9", "18/10/2023"),
                new Automate("auto10", "18/10/2023"),
                new Automate("auto11", "18/10/2023")
            };

            AumateList.ItemsSource = items;

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
        private async void SignOut(object sender, RoutedEventArgs e)
    }
            await this.sessionVM!.SignOutAsync();
    public class Automate
    {
        private string name;
        private string date;
        public string Name { get => this.name; set => this.name = value; }
        public string Date { get => this.date; set => this.date = value; }
        public Automate(string name, string date)
        {
            this.name = name;
            this.date = date;
        }
    }
}
>>>>>>>>> Temporary merge branch 2
