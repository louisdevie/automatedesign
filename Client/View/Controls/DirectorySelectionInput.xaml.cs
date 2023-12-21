using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using AutomateDesign.Client.View.Helpers;
using UserControl = System.Windows.Controls.UserControl;

namespace AutomateDesign.Client.View.Controls;

public partial class DirectorySelectionInput : UserControl
{
    public static readonly DependencyProperty PathProperty =
        DependencyProperty.Register("Path", typeof(string), typeof(DirectorySelectionInput));

    public string Path
    {
        get => (string)GetValue(PathProperty);
        set => SetValue(PathProperty, value);
    }

    public DirectorySelectionInput()
    {
        InitializeComponent();
    }

    private void BrowseButtonClick(object sender, RoutedEventArgs e)
    {
        var folderBrowserDialog = new FolderBrowserDialog
        {
            Description = "Sélectionnez un dossier",
            UseDescriptionForTitle = true,
            SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                           + System.IO.Path.DirectorySeparatorChar,
            ShowNewFolderButton = true
        };

        if (folderBrowserDialog.ShowDialog(Window.GetWindow(this)?.GetIWin32Window()) == DialogResult.OK)
        {
            this.Path = folderBrowserDialog.SelectedPath;
        }
    }
}