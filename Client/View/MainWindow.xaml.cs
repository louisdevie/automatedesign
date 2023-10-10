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
using System.Windows.Shapes;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.ChangementFenetre(new LoginView(this));
        }

        /// <summary>
        /// Appeler par les pages pour changer la page a afficher
        /// </summary>
        /// <param name="page">la nouvelle page a afficher</param>
        public void ChangementFenetre(Page page)
        {
            this.frame.Content = page;
        }
    }
}
