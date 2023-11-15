﻿using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Client.View.Navigation;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AutomateDesign.Client.View
{
    /// <summary>
    /// Logique d'interaction pour ChangePasswordPopup.xaml
    /// </summary>
    public partial class ChangePasswordPopup : Window, INavigationContainer
    {
        private Navigator navigator;

        public ChangePasswordPopup(Session session)
        {
            InitializeComponent();

            this.navigator = new(this, new ChangePasswordView());
            this.navigator.Session = session;

            WindowPreferences.ApplySize(WindowPreferences.WindowSize.Small, this);
            WindowPreferences.ApplyResizeMode(WindowPreferences.ResizeMode.MinimizeOnly, this);
        }

        public Window Window => this;

        public void ApplyPreferences(WindowPreferences preferences)
        {
            preferences.ApplyTitleTo(this);
        }

        public void ChangeContent(INavigable value)
        {
            this.daFrame.Content = value;
        }
    }
}
