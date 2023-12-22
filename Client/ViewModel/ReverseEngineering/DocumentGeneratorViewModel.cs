using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutomateDesign.Client.Model.ReverseEngineering;
using AutomateDesign.Client.Model.ReverseEngineering.Items;
using AutomateDesign.Client.ViewModel.Documents;
using Microsoft.CodeAnalysis;

namespace AutomateDesign.Client.ViewModel.ReverseEngineering;

/// <summary>
/// Le modèle-vue d'une opération de rétro-génération.
/// </summary>
public class DocumentGeneratorViewModel : BaseViewModel
{
    private readonly DocumentGenerator model;
    private readonly Action debouncedReload;
    private string sourceDirectory;
    private bool loadingFinished;

    public string SourceDirectory
    {
        get => this.sourceDirectory;
        set
        {
            this.sourceDirectory = value;
            this.LoadingFinished = false;
            this.debouncedReload();
            this.NotifyPropertyChanged();
        }
    }

    public IEnumerable<ImportedItemViewModel> ImportedItems =>
        this.model.ImportedItems.Select(item => new ImportedItemViewModel(item));

    public bool LoadingFinished
    {
        get => this.loadingFinished;
        private set
        {
            this.loadingFinished = value;
            this.NotifyPropertyChanged();
        }
    }

    public DocumentGeneratorViewModel(ExistingDocumentViewModel documentViewModel)
    {
        this.sourceDirectory = string.Empty;
        this.model = new DocumentGenerator(documentViewModel.Document);
        this.model.AddModificationObserver(documentViewModel);
        this.debouncedReload = Timing.DebounceAsync(this.Reload, TimeSpan.FromMilliseconds(500));
        this.loadingFinished = true;
    }

    private async Task Reload()
    {
        await this.model.LoadDirectoryAsync(this.sourceDirectory);
        this.NotifyPropertyChanged(nameof(ImportedItems));
        this.LoadingFinished = true;
    }

    public void Generate()
    {
        this.model.Generate();
    }
}