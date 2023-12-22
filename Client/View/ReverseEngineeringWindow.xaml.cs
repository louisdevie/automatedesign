using System.Windows;
using AutomateDesign.Client.ViewModel.Documents;
using AutomateDesign.Client.ViewModel.ReverseEngineering;

namespace AutomateDesign.Client.View;

public partial class ReverseEngineeringWindow : Window
{
    private readonly DocumentGeneratorViewModel viewModel;
    
    public ReverseEngineeringWindow(ExistingDocumentViewModel documentVM)
    {
        this.viewModel = new DocumentGeneratorViewModel(documentVM);

        DataContext = this.viewModel;
        InitializeComponent();
    }

    private void FinishButtonClick(object sender, RoutedEventArgs e)
    {
        this.viewModel.Generate();
        this.Close();
    }

    private void CancelButtonClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}