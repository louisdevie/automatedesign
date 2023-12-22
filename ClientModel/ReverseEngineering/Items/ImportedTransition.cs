using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

internal class ImportedTransition : ImportedItem
{
    private ImportedState start, end;
    private ImportedEvent triggeredBy;
    private Transition? generatedTransition;

    /// <summary>
    /// L'état de départ.
    /// </summary>
    public ImportedState Start => this.start;
    
    /// <summary>
    /// L'état d'arrivée.
    /// </summary>
    public ImportedState End => this.end;
    
    /// <summary>
    /// L'évènement qui déclenche la transition.
    /// </summary>
    public ImportedEvent TriggeredBy => this.triggeredBy;

    public override string Description => $"Transition de {this.Start.Name} à {this.End.Name} (déclenchée par {this.TriggeredBy.Name})";

    /// <summary>
    /// Crée une nouvelle transition à importer.
    /// </summary>
    /// <param name="start">L'état de départ.</param>
    /// <param name="end">L'état d'arrivée.</param>
    /// <param name="triggeredBy">L'évènement qui déclenche la transition.</param>
    public ImportedTransition(ImportedState start, ImportedState end, ImportedEvent triggeredBy)
    {
        this.start = start;
        this.end = end;
        this.triggeredBy = triggeredBy;
    }

    public override void Generate(DocumentGenerator documentGenerator)
    {
        this.start.Generate(documentGenerator);
        this.end.Generate(documentGenerator);
        this.triggeredBy.Generate(documentGenerator);
        
        this.generatedTransition ??= documentGenerator.GenerateTransition(
            this.start.GeneratedState!,
            this.end.GeneratedState!,
            this.triggeredBy.GeneratedEvent!
        );
    }
}