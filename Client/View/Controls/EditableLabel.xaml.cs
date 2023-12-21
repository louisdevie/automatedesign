using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutomateDesign.Client.View.Controls
{
    /// <summary>
    /// Logique d'interaction pour EditableLabel.xaml
    /// </summary>
    public partial class EditableLabel : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableLabel));

        /// <summary>
        /// Le texte contenu dans le label.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty EditingProperty =
            DependencyProperty.Register("Editing", typeof(bool), typeof(EditableLabel));

        /// <summary>
        /// Le texte contenu dans le label.
        /// </summary>
        public bool Editing
        {
            get { return (bool)GetValue(EditingProperty); }
            set { SetValue(EditingProperty, value); }
        }

        public EditableLabel()
        {
            InitializeComponent();
        }

        private static readonly Uri CheckIconUri = new("/Resources/Icons/check.png", UriKind.RelativeOrAbsolute);
        private static readonly Uri PencilIconUri = new("/Resources/Icons/pencil.png", UriKind.RelativeOrAbsolute);

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Editing)
            {
                this.buttonIcon.Source = new BitmapImage(PencilIconUri);
                this.Editing = false;
            }
            else
            {
                this.buttonIcon.Source = new BitmapImage(CheckIconUri);
                this.Editing = true;
                this.input.Focus();
            }
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Enter)
            {
                EditButtonClick(sender, e);
            }
        }
    }
}
