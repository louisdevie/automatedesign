using System.Text;
using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Export;

/// <summary>
/// Un index contenant des identifiants d'états et d'évènements.
/// </summary>
public class DocumentIndex
{
    private record IndexEntry(string Identifier);

    private HashSet<IndexEntry> statesIndex;
    private HashSet<IndexEntry> eventsIndex;
    private Dictionary<int, IndexEntry> statesById;
    private Dictionary<int, IndexEntry> eventsById;

    /// <summary>
    /// Crée un index vide.
    /// </summary>
    public DocumentIndex()
    {
        this.statesIndex = new HashSet<IndexEntry>();
        this.statesById = new Dictionary<int, IndexEntry>();
        this.eventsIndex = new HashSet<IndexEntry>();
        this.eventsById = new Dictionary<int, IndexEntry>();
    }

    /// <summary>
    /// Crée un index à partir des éléments d'un automate.
    /// </summary>
    /// <param name="document">L'automate à partir duquel générer l'index.</param>
    public DocumentIndex(Document document) : this()
    {
        foreach (State state in document.States)
        {
            this.RegisterState(state);
        }

        foreach (EnumEvent evt in document.Events)
        {
            this.RegisterEvent(evt);
        }
    }

    private static string ConvertToIdentifierWithDiscriminator(string name, int discriminator)
    {
        var sb = new StringBuilder();
        sb.Append(TextNormaliser.ToIdentifier(name));
        if (discriminator > 1) sb.Append(discriminator);
        return sb.ToString();
    }

    private static void Register(HashSet<IndexEntry> index, Dictionary<int, IndexEntry> idIndex, int id, string name)
    {
        // on essaie d'ajouter l'identifiant en augmentant le discriminant jusqu'à ce qu'il soit disponible
        // pour garantir l'unicité (si deux éléments s'appellent A, on aura A et A2 par exemple)
        int discriminator = 0;
        IndexEntry entry;
        do
        {
            discriminator++;
            entry = new IndexEntry(ConvertToIdentifierWithDiscriminator(name, discriminator));
        } while (index.Contains(entry));
        
        index.Add(entry);
        idIndex.Add(id, entry);
    }

    /// <summary>
    /// Ajoute un évènement à l'index.
    /// </summary>
    /// <param name="evt">L'évènement à ajouter.</param>
    public void RegisterEvent(EnumEvent evt) => Register(this.eventsIndex, this.eventsById, evt.Id, evt.Name);

    /// <summary>
    /// Ajoute un état à l'index.
    /// </summary>
    /// <param name="state">L'état à ajouter.</param>
    public void RegisterState(State state) => Register(this.statesIndex, this.statesById, state.Id, state.Name);

    /// <summary>
    /// Récupère l'identifiant correspondant à un évènement.
    /// </summary>
    /// <param name="evt">L'évènement à rechercher.</param>
    /// <returns>L'identifiant trouvé dans l'index.</returns>
    /// <exception cref="KeyNotFoundException">Si l'évènement n'est pas dans l'index.</exception>
    public string LookUpEventIdentifier(EnumEvent evt) => this.eventsById[evt.Id].Identifier;
    
    /// <summary>
    /// Récupère l'identifiant correspondant à un état.
    /// </summary>
    /// <param name="state">L'état à rechercher.</param>
    /// <returns>L'identifiant trouvé dans l'index.</returns>
    /// <exception cref="KeyNotFoundException">Si l'état n'est pas dans l'index.</exception>
    public string LookUpStateIdentifier(State state) => this.statesById[state.Id].Identifier;
}