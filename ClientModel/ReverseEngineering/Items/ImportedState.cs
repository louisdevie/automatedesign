using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

/// <summary>
/// Un état importé depuis du code.
/// </summary>
internal class ImportedState : ImportedItem
{
    private string name;
    private StateKind kind;

    /// <summary>
    /// Le nom de l'état dans le code.
    /// </summary>
    public string Name => this.name;

    /// <summary>
    /// Le type de l'état.
    /// </summary>
    public StateKind Kind => this.kind;

    /// <summary>
    /// Crée un nouvel état à importer.
    /// </summary>
    /// <param name="name">Le nom de l'état à importer.</param>
    /// <param name="kind">Le type de l'état à importer s'il est connu.</param>
    public ImportedState(string name, StateKind kind = StateKind.Normal)
    {
        this.name = name;
        this.kind = kind;
    }

    /// <summary>
    /// Fusionne les information d'un autre état avec celui-ci.
    /// </summary>
    /// <param name="other">L'autre état à fusionner dans celui-ci.</param>
    public void Merge(ImportedState other)
    {
        this.kind = other.kind;
    }
}