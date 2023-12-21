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

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Logique d'interaction pour InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        private string userInput;

        public string UserInput => this.userInput;

        public InputDialog(string title, string prompt)
        {
            this.userInput = string.Empty;
            this.Loaded += this.SelfLoaded;

            InitializeComponent();
            this.Title = title;
            this.prompt.Text = prompt;
        }

        private void SelfLoaded(object sender, RoutedEventArgs e)
        {
            this.inputTextBox.Focus();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.userInput = this.inputTextBox.Text;
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        /// <summary>
        /// Déclenche le clic sur le bouton Ok en cas d'appuie sur la touche entrer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.OkButtonClick(sender, e);
            }
        }
    }
}
