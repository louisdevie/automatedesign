using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

internal class ImportedDefaultEvent : ImportedEvent
{
    public override string Description => "Évènement par défaut";

    public override string Name => "défaut";

    public override IEvent GeneratedEvent => new DefaultEvent();

    public override void Generate(DocumentGenerator documentGenerator) { /* pas d'action de génération */ }
}