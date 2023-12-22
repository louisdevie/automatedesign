using AutomateDesign.Client.Model.ReverseEngineering.Items;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering;

/// <summary>
/// Un index des éléments importés depuis un programme d'automate existant.
/// </summary>
internal class ImportedItemsIndex
{
    private readonly Dictionary<string, ImportedState> statesIndex = new();
    private readonly Dictionary<string, ImportedEnumEvent> eventsIndex = new();
    private readonly ImportedDefaultEvent defaultEvent = new();
    private readonly List<ImportedTransition> transitions = new();

    /// <summary>
    /// Renvoie l'état qui correspond à un nom ou le crée s'il est inconnu.
    /// </summary>
    /// <param name="name">Le nom de l'état recherché.</param>
    /// <returns>Un <see cref="ImportedState"/> existant ou un nouveau.</returns>
    public ImportedState FindOrCreateState(string name)
    {
        ImportedState state;

        if (this.statesIndex.TryGetValue(name, out var stateFound))
        {
            state = stateFound;
        }
        else
        {
            state = new ImportedState(name);
            this.statesIndex.Add(name, state);
        }

        return state;
    }

    /// <summary>
    /// Crée ou mets à jour les informations d'un état.
    /// </summary>
    /// <param name="name">Le nom de l'état.</param>
    /// <param name="kind">Le type d'état s'il est connu.</param>
    public void CreateOrUpdateState(string name, StateKind kind = StateKind.Normal)
    {
        ImportedState state = new(name, kind);

        if (this.statesIndex.TryGetValue(name, out var stateFound))
        {
            stateFound.Merge(state);
        }
        else
        {
            this.statesIndex.Add(name, state);
        }
    }

    /// <summary>
    /// Renvoie l'évènement qui correspond à un nom ou le crée s'il est inconnu.
    /// </summary>
    /// <param name="name">Le nom de l'évènement recherché.</param>
    /// <returns>Un <see cref="ImportedEnumEvent"/> existant ou un nouveau.</returns>
    public ImportedEnumEvent FindOrCreateEnumEvent(string name)
    {
        ImportedEnumEvent evt;

        if (this.eventsIndex.TryGetValue(name, out var stateFound))
        {
            evt = stateFound;
        }
        else
        {
            evt = new ImportedEnumEvent(name);
            this.eventsIndex.Add(name, evt);
        }

        return evt;
    }

    /// <summary>
    /// Crée un évènement s'il n'existe pas déjà.
    /// </summary>
    /// <param name="name">Le nom de l'évènement.</param>
    public void CreateOrUpdateEnumEvent(string name)
    {
        this.eventsIndex.TryAdd(name, new ImportedEnumEvent(name));
    }

    /// <summary>
    /// Crée une transition déclenchée par un évènement défini par l'utilisateur.
    /// </summary>
    /// <param name="start">Le nom de l'état de départ.</param>
    /// <param name="end">Le nom de l'état d'arrivée.</param>
    /// <param name="evt">Le nom de l'évènement.</param>
    public void CreateTransitionTriggeredByEnumEvent(string start, string end, string evt)
    {
        this.transitions.Add(
            new ImportedTransition(
                this.FindOrCreateState(start),
                this.FindOrCreateState(end),
                this.FindOrCreateEnumEvent(evt)
            )
        );
    }

    /// <summary>
    /// Crée une transition déclenchée par défaut.
    /// </summary>
    /// <param name="start">Le nom de l'état de départ.</param>
    /// <param name="end">Le nom de l'état d'arrivée.</param>
    public void CreateTransitionTriggeredByDefaultEvent(string start, string end)
    {
        if (start != end) // les transitions d'un état à lui-même par défaut est implicite
        {
            this.transitions.Add(
                new ImportedTransition(
                    this.FindOrCreateState(start),
                    this.FindOrCreateState(end),
                    this.defaultEvent
                )
            );
        }
    }

    /// <summary>
    /// Efface tout le contenu de l'index.
    /// </summary>
    public void Clear()
    {
        this.statesIndex.Clear();
        this.eventsIndex.Clear();
        this.transitions.Clear();
    }

    public IEnumerable<ImportedItem> GetAllItems()
    {
        foreach (ImportedState state in this.statesIndex.Values) yield return state;
        foreach (ImportedEnumEvent evt in this.eventsIndex.Values) yield return evt;
        foreach (ImportedTransition transition in transitions) yield return transition;
    }
}