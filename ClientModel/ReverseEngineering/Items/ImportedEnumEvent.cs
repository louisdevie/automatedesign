using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

/// <summary>
/// Un évènement importé depuis du code.
/// </summary>
class ImportedEnumEvent : ImportedEvent
{
    private string name;
    private EnumEvent? generatedEvent;

    public override string Name => this.name;

    public override IEvent? GeneratedEvent => this.generatedEvent;

    public override string Description => $"Évènement {this.Name}";

    public override void Generate(DocumentGenerator documentGenerator)
    {
        this.generatedEvent ??= documentGenerator.GenerateEnumEvent(this.name);
    }

    /// <summary>
    /// Crée un nouvel évènement à importer.
    /// </summary>
    /// <param name="name">Le nom de l'évènement.</param>
    public ImportedEnumEvent(string name)
    {
        this.name = name;
    }
}