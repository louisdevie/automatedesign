namespace AutomateDesign.Client.Model.ReverseEngineering.CSharp;

/// <summary>
/// Le contexte dans lequel une transition est effectuée dans du code C#.
/// </summary>
internal class CSharpTransition
{
    public enum Switch
    {
        /// <summary>
        /// En dehors d'un switch.
        /// </summary>
        Outside,

        /// <summary>
        /// Dans un switch après un label case avec une valeur.
        /// </summary>
        Case,

        /// <summary>
        /// Dans un switch après un label default.
        /// </summary>
        Default,
    }

    private string currentState, nextState;
    private Switch switchContext;
    private string? switchEventName;

    /// <summary>
    /// Crée une transition potentielle.
    /// </summary>
    /// <param name="currentState">Le nom de l'état dans lequel on se trouve.</param>
    /// <param name="nextState">Le nom de l'état suivant.</param>
    /// <param name="switchContext">Le contexte de déclaration switch.</param>
    /// <param name="switchEventName">La valeur du case dans le cas ou on est dans un switch.</param>
    public CSharpTransition(string currentState, string nextState, Switch switchContext, string? switchEventName)
    {
        this.currentState = currentState;
        this.nextState = nextState;
        this.switchContext = switchContext;
        this.switchEventName = switchEventName;
    }

    /// <summary>
    /// Ajoute cette transition dans un index.
    /// </summary>
    /// <param name="index">L'index dans lequel ajouter cette transition.</param>
    public void AddTo(ImportedItemsIndex index)
    {
        if (this.switchContext == Switch.Case)
        {
            index.CreateTransitionTriggeredByEnumEvent(
                this.currentState,
                this.nextState,
                this.switchEventName!
            );
        }
        else
        {
            index.CreateTransitionTriggeredByDefaultEvent(
                this.currentState,
                this.nextState
            );
        }
    }
}