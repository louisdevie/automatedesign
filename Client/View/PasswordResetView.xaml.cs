﻿using System;
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
    /// Logique d'interaction pour PasswordResetView.xaml
    /// </summary>
    public partial class PasswordResetView : Page
    {
        private MainWindow mainWindow;
        public PasswordResetView(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
    }
}