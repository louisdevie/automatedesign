using System.Diagnostics.CodeAnalysis;

namespace AutomateDesign.Client.Model.ReverseEngineering.CSharp;

/// <summary>
/// Les informations sur le contexte d'un parcours d'un arbre syntaxique C#
/// </summary>
internal class CSharpContext
{
    public enum Class
    {
        /// <summary>
        /// En dehors de n'importe quelle classe.
        /// </summary>
        Outside,

        /// <summary>
        /// Dans une classe d'état.
        /// </summary>
        State,

        /// <summary>
        /// Dans une classe sans intérêt.
        /// </summary>
        Other
    }

    public enum Method
    {
        /// <summary>
        /// En dehors d'une méthode.
        /// </summary>
        Outside,

        /// <summary>
        /// Dans une méthode de transition.
        /// </summary>
        Transition,

        /// <summary>
        /// Dans une méthode sans intérêt.
        /// </summary>
        Other
    }

    private readonly Stack<(Class, string)> classStack = new();
    private Method currentMethod = Method.Outside;
    
    private CSharpTransition.Switch currentSwitchLabelKind = CSharpTransition.Switch.Outside;
    private string? currentSwitchLabelEventName;

    /// <summary>
    /// La classe à l'intérieur de laquelle on se trouve.
    /// </summary>
    public Class CurrentClassKind => this.classStack.Count == 0 ? Class.Outside : this.classStack.Peek().Item1;

    public string CurrentClassName => this.classStack.Peek().Item2;
    
    /// <summary>
    /// La méthode à l'intérieur de laquelle on se trouve.
    /// </summary>
    public Method CurrentMethod => this.currentMethod;

    /// <summary>
    /// Le type de label de déclaration switch en-dessous de laquelle on se trouve.
    /// </summary>
    public CSharpTransition.Switch CurrentSwitchLabelKind => this.currentSwitchLabelKind;

    /// <summary>
    /// La valeur du label de déclaration switch en-dessous de laquelle on se trouve.
    /// </summary>
    public string? CurrentSwitchLabelEventName => this.currentSwitchLabelEventName;
    
    /// <summary>
    /// Indique qu'on rentre dans une classe.
    /// </summary>
    /// <param name="cls">Le type de classe.</param>
    /// <param name="name">Le nom de la classe.</param>
    public void PushClass(Class cls, string name)
    {
        this.classStack.Push((cls, name));
    }

    /// <summary>
    /// Indique qu'on sort d'une classe.
    /// </summary>
    public void PopClass()
    {
        this.classStack.Pop();
    }

    /// <summary>
    /// Indique qu'on entre dans une méthode.
    /// </summary>
    /// <param name="methodContext">Le type de méthode.</param>
    public void PushMethod(Method methodContext)
    {
        this.currentMethod = methodContext;
    }

    /// <summary>
    /// Indique qu'on sort d'une méthode.
    /// </summary>
    public void PopMethod()
    {
        this.currentMethod = Method.Outside;
    }

    /// <summary>
    /// Indique qu'on a passé un label case dans switch.
    /// </summary>
    /// <param name="value">La valeur du case.</param>
    public void PushCaseSwitchLabel(string value)
    {
        this.currentSwitchLabelKind = CSharpTransition.Switch.Case;
        this.currentSwitchLabelEventName = value;
    }
    
    /// <summary>
    /// Indique qu'on a passé un label default dans switch.
    /// </summary>
    public void PushDefaultSwitchLabel()
    {
        this.currentSwitchLabelKind = CSharpTransition.Switch.Default;
        this.currentSwitchLabelEventName = null;
    }

    /// <summary>
    /// Indique qu'on est sorti d'un switch.
    /// </summary>
    public void PopSwitch()
    {
        this.currentSwitchLabelKind = CSharpTransition.Switch.Outside;
        this.currentSwitchLabelEventName = null;
    }
}