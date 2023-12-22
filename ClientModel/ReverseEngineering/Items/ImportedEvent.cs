using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

public abstract class ImportedEvent : ImportedItem
{
    /// <summary>
    /// Le nom de l'évènement.
    /// </summary>
    public abstract string Name { get; }

    public abstract IEvent? GeneratedEvent { get; }
}