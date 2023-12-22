using AutomateDesign.Client.Model.ReverseEngineering.Items;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutomateDesign.Client.ViewModel.ReverseEngineering;

public class ImportedItemViewModel : BaseViewModel
{
    private ImportedItem model;

    public bool WillBeImported
    {
        get => this.model.WillBeImported;
        set
        {
            this.model.WillBeImported = value;
            this.NotifyPropertyChanged();
        }
    }

    public string Description => this.model.Description;

    public ImportedItemViewModel(ImportedItem model)
    {
        this.model = model;
    }
}