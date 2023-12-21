using AutomateDesign.Client.Model.ReverseEngineering.CSharp;
using AutomateDesign.Client.Model.ReverseEngineering.Items;

namespace AutomateDesign.Client.Model.ReverseEngineering;

/// <summary>
/// Une génération d'automate à partir de code source existant.
/// </summary>
public class DocumentGeneration
{
    private IFileParser[] parsers;
    private ImportedItem[] importedItems;

    private static IFileParser[] DefaultParsers => new[]
    {
        new CSharpFileParser() as IFileParser
    };

    /// <summary>
    /// Crée un nouveau générateur d'automates.
    /// </summary>
    public DocumentGeneration()
    {
        this.parsers = DefaultParsers;
        this.importedItems = Array.Empty<ImportedItem>();
    }

    /// <summary>
    /// Charge tous les éléments possibles depuis un dossier.
    /// </summary>
    /// <param name="path">Le chemin du dossier à charger.</param>
    public Task LoadDirectoryAsync(string path)
    {
        return Task.Run(() =>
        {
            var index = new ImportedItemsIndex();
            this.FindItemsInDirectory(path, index);
            this.importedItems = index.GetAllItems().ToArray();
        });
    }

    private void FindItemsInDirectory(string path, ImportedItemsIndex index)
    {
        foreach (string file in Directory.EnumerateFiles(path))
        {
            this.FindItemsInFile(file, index);
        }

        foreach (string subDirectory in Directory.EnumerateDirectories(path))
        {
            this.FindItemsInDirectory(subDirectory, index);
        }
    }

    private void FindItemsInFile(string file, ImportedItemsIndex index)
    {
        var parser = this.parsers.FirstOrDefault(parser => parser.CanParse(file));

        if (parser != null)
        {
            parser.TryParse(File.ReadAllText(file), index);
        }
    }
}