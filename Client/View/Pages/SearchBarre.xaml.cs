using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using AutomateDesign.Client.ViewModel.Documents;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace AutomateDesign.Client.View.Pages
{
    /// <summary>
    /// Logique d'interaction pour SearchBarre.xaml
    /// </summary>
    public partial class SearchBarre : UserControl
    {
        private DocumentCollectionViewModel documentsVM;

        public SearchBarre()
        {
            InitializeComponent();
            DataContext = this;
            this.documentsVM = new DocumentCollectionViewModel();
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

        private void DeleteSearchButtonClick(object sender, RoutedEventArgs e)
        {
            TextBoxRecherche.Text = string.Empty;
        }

        private void SearchAutomate(object sender, RoutedEventArgs e)
        {
            string searchText = TextBoxRecherche.Text;


        }
    }
}
