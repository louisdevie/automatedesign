using AutomateDesign.Client.Model;
using AutomateDesign.Client.View.Navigation;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour ChangePasswordPopup.xaml
    /// </summary>
    public partial class ChangePasswordPopup : Window
    {
        private Navigator navigator;

        public ChangePasswordPopup(Session session)
        {
            InitializeComponent();

            this.navigator = new(this, this.daFrame, new ChangePasswordView());
            this.navigator.Session = session;
        }
    }
}
