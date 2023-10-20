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
            ProfilMenu.Visibility = Visibility.Collapsed;
        }

        private void BurgerToggleButton_Click(object sender, RoutedEventArgs e)
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

        private void LogOutButton(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePwdButton(object sender, RoutedEventArgs e)
        {

        }

        private Point lastMousePosition;

        private void AddStateButtonClick(object sender, RoutedEventArgs e)
        {
            // Créez un Grid pour organiser le cercle et le texte
            Grid grid = new Grid();
            grid.Width = 100; 
            grid.Height = 100;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            // Créez un Ellipse (cercle)
            Ellipse cercle = new Ellipse
            {
                Width = 100,
                Height = 100,
                Stroke = Brushes.Black, // Couleur du bord (noir)
                StrokeThickness = 2, // Épaisseur du trait
                Fill = Brushes.White, // Couleur de remplissage (blanc)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            // Créez un TextBlock pour ajouter du texte au cercle
            TextBox texte = new TextBox
            {
                Text = "Etat Test",
                Foreground = Brushes.Black, // Couleur du texte (noir)
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            // Gestionnaire d'événement lorsque la souris est enfoncée sur le Grid
            grid.MouseLeftButtonDown += (s, e) =>
            {
                lastMousePosition = e.GetPosition(grid);
                grid.CaptureMouse();
            };

            // Gestionnaire d'événement lorsque la souris est déplacée
            grid.MouseMove += (s, e) =>
            {
                if (grid.IsMouseCaptured)
                {
                    Point currentPosition = e.GetPosition(grid);
                    double offsetX = currentPosition.X - lastMousePosition.X;
                    double offsetY = currentPosition.Y - lastMousePosition.Y;

                    // Déplacez le Grid en fonction de la différence de position
                    Canvas.SetLeft(grid, Canvas.GetLeft(grid) + offsetX);
                    Canvas.SetTop(grid, Canvas.GetTop(grid) + offsetY);

                    lastMousePosition = currentPosition;
                }
            };

            // Gestionnaire d'événement lorsque le bouton de la souris est relâché
            grid.MouseLeftButtonUp += (s, e) =>
            {
                grid.ReleaseMouseCapture();
            };

            // Ajoutez le cercle et le texte au Grid
            grid.Children.Add(cercle);
            grid.Children.Add(texte);

            // Ajoutez le Grid au Canvas
            ZoneDessinAutomate.Children.Add(grid);

        }

        
    }
}
