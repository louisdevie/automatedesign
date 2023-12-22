using AutomateDesign.Client.Model.Logic.Editor;
using AutomateDesign.Client.Model.ReverseEngineering.CSharp;
using AutomateDesign.Client.Model.ReverseEngineering.Items;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering;

/// <summary>
/// Une génération d'automate à partir de code source existant.
/// </summary>
public class DocumentGenerator
{
    private readonly Document document;
    private IFileParser[] parsers;
    private ImportedItem[] importedItems;
    private List<IModificationsObserver> modificationsObservers;
    private int numberOfStatesGenerated;

    public IEnumerable<ImportedItem> ImportedItems => this.importedItems;

    private static IFileParser[] DefaultParsers => new[]
    {
        new CSharpFileParser() as IFileParser
    };

    /// <summary>
    /// Crée un nouveau générateur d'automates.
    /// </summary>
    /// <param name="document">Le document dans lequel générer les éléments.</param>
    public DocumentGenerator(Document document)
    {
        this.document = document;
        this.parsers = DefaultParsers;
        this.importedItems = Array.Empty<ImportedItem>();
        this.modificationsObservers = new List<IModificationsObserver>();
        this.numberOfStatesGenerated = 0;
    }

    /// <summary>
    /// Permets à un objet d'observer les modification apportées par la rétro-génération.
    /// </summary>
    public void AddModificationObserver(IModificationsObserver modificationsObserver)
    {
        this.modificationsObservers.Add(modificationsObserver);
    }

    /// <summary>
    /// Détache un objet qui observe les modification apportées par la rétro-génération.
    /// </summary>
    public void RemoveModificationsObserver(IModificationsObserver modificationsObserver)
    {
        this.modificationsObservers.Remove(modificationsObserver);
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

    /// <summary>
    /// Rajoute les éléments dans l'automate.
    /// </summary>
    public void Generate()
    {
        foreach (ImportedItem item in this.importedItems)
        {
            if (item.WillBeImported)
            {
                item.Generate(this);
            }
        }
    }

    internal State GenerateState(string name, StateKind kind)
    {
        State generated = this.document.CreateState(
            name,
            State.CenteredAt(100, 100 + 200 * this.numberOfStatesGenerated),
            kind
        );
        this.numberOfStatesGenerated++;
        
        foreach (var observer in this.modificationsObservers) observer.OnStateAdded(generated);

        return generated;
    }

    public EnumEvent GenerateEnumEvent(string name)
    {
        EnumEvent generated = this.document.CreateEnumEvent(name);
        
        foreach (var observer in this.modificationsObservers) observer.OnEnumEventAdded(generated);

        return generated;
    }

    public Transition GenerateTransition(State from, State to, IEvent triggeredBy)
    {
        Transition generated = this.document.CreateTransition(from, to, triggeredBy);
        
        foreach (var observer in this.modificationsObservers) observer.OnTransitionAdded(generated);

        return generated;
    }
}