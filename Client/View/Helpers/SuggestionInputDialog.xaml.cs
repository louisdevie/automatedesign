using System.Collections.Generic;
using System.Windows;

namespace AutomateDesign.Client.View.Helpers
{
    /// <summary>
    /// Logique d'interaction pour InputDialog.xaml
    /// </summary>
    public partial class SuggestionInputDialog : Window
    {
        private string userInput;
        private IEnumerable<string> suggestions;

        public string UserInput => this.userInput;

        public SuggestionInputDialog(string title, string prompt, IEnumerable<string> suggestions)
        {
            this.userInput = string.Empty;
            this.suggestions = suggestions;

            InitializeComponent();
            this.Title = title;
            this.prompt.Text = prompt;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.userInput = this.inputComboBox.Text;
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
