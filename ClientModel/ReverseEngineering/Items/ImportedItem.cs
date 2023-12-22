using AutomateDesign.Client.Model.Logic.Editor;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

/// <summary>
/// Un élément d'un automate importé depuis du code.
/// </summary>
public abstract class ImportedItem
{
    private bool willBeImported = true;
    
    /// <summary>
    /// S'il faut importer cet élément ou non.
    /// </summary>
    public bool WillBeImported
    {
        get => this.willBeImported;
        set => this.willBeImported = value;
    }
    
    /// <summary>
    /// Une description de cet élément.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Rajoute cet élément dans un automate. Cette méthode peut être appelée plusieurs fois,
    /// l'élément doit donc retenir s'il a déjà été généré.
    /// </summary>
    /// <param name="documentGenerator">Le générateur à utiliser pour créer l'élément.</param>
    public abstract void Generate(DocumentGenerator documentGenerator);
}