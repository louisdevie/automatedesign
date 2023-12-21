namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

/// <summary>
/// Un évènement importé depuis du code.
/// </summary>
class ImportedEnumEvent : ImportedEvent
{
    private string name;

    /// <summary>
    /// Le nom de l'évènement.
    /// </summary>
    public string Name => this.name;

    /// <summary>
    /// Crée un nouvel évènement à importer.
    /// </summary>
    /// <param name="name">Le nom de l'évènement.</param>
    public ImportedEnumEvent(string name)
    {
        this.name = name;
    }
}