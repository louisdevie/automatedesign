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

    public enum SwitchLabel
    {
        /// <summary>
        /// Un label case avec une valeur.
        /// </summary>
        Case,

        /// <summary>
        /// Le label par défaut.
        /// </summary>
        Default,
    }

    private Stack<Class> classStack = new();
    private Method currentMethod = Method.Outside;
    private SwitchLabel? currentSwitchLabelType;
    private string? currentSwitchLabelValue;

    /// <summary>
    /// La classe à l'intérieur de laquelle on se trouve.
    /// </summary>
    public Class CurrentClass => this.classStack.Count == 0 ? Class.Outside : this.classStack.Peek();

    /// <summary>
    /// La méthode à l'intérieur de laquelle on se trouve.
    /// </summary>
    public Method CurrentMethod => this.currentMethod;

    /// <summary>
    /// Teste si le label au-dessus du code actuel est un case et renvoie la valeur le cas échéant.
    /// </summary>
    /// <param name="value">La valeur du case, si c'en est un.</param>
    /// <returns><see langword="true"/> si c'est un label de type case.</returns>
    public bool CurrentSwitchLabelIsCase([NotNullWhen(true)] out string? value)
    {
        bool result = this.currentSwitchLabelType == SwitchLabel.Case;
        value = result ? this.currentSwitchLabelValue : null;
        return result;
    }

    /// <summary>
    /// Teste si le label au-dessus du code actuel est un default.
    /// </summary>
    /// <returns><see langword="true"/> si c'est un label de type default.</returns>
    public bool CurrentSwitchLabelIsDefault()
    {
        return this.currentSwitchLabelType == SwitchLabel.Default;
    }

    /// <summary>
    /// Indique qu'on rentre dans une classe.
    /// </summary>
    /// <param name="cls">Le type de classe.</param>
    public void PushClass(Class cls)
    {
        this.classStack.Push(cls);
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
        this.currentSwitchLabelType = SwitchLabel.Case;
        this.currentSwitchLabelValue = value;
    }
}