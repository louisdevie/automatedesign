﻿using AutomateDesign.Client.View.Controls;
using AutomateDesign.Client.View.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class HomeView : NavigablePage
    {
        private List<Automate> items;
        public override WindowPreferences Preferences => new(
            WindowPreferences.WindowSize.FullScreen,
            WindowPreferences.ResizeMode.Resizeable
        );

        public HomeView()
        {
            InitializeComponent();
            items = new List<Automate>();
            items.Add(new Automate("auto1", "16/10/2023"));
            items.Add(new Automate("auto2", "17/10/2023"));
            items.Add(new Automate("auto3", "18/10/2023"));
            items.Add(new Automate("auto4", "18/10/2023"));
            items.Add(new Automate("auto5", "18/10/2023"));
            items.Add(new Automate("auto6", "18/10/2023"));
            items.Add(new Automate("auto7", "18/10/2023"));
            items.Add(new Automate("auto8", "18/10/2023"));
            items.Add(new Automate("auto9", "18/10/2023"));
            items.Add(new Automate("auto10", "18/10/2023"));
            items.Add(new Automate("auto11", "18/10/2023"));

            AumateList.ItemsSource = items;
            ProfilMenu.Visibility = Visibility.Hidden;
           
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
            if (this.TextBoxRecherche.Text == "") {
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

        private void ChangePwdButton(object sender, RoutedEventArgs e)
        {

        }

        private void LogOutButton(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSearchButtonClick(object sender, RoutedEventArgs e)
        {
            TextBoxRecherche.Text=string.Empty;
        }
    }

    public class Automate
    {
        private string name;
        private string date;
        public string Name { get => this.name; set => this.name = value; }
        public string Date { get => this.date; set => this.date = value; }
        public Automate(string name, string date) {
            this.name = name;
            this.date = date;
        }

     
    }
}
