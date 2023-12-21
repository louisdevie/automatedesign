namespace AutomateDesign.Client.Model.ReverseEngineering.Items;

internal class ImportedTransition : ImportedItem
{
    private ImportedState start, end;
    private ImportedEvent triggeredBy;

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
}