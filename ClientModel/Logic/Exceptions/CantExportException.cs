namespace AutomateDesign.Client.Model.Logic.Exceptions;

/// <summary>
/// Exception levé quand un problème lié à l'export survient.
/// </summary>
public class CantExportException : Exception
{
    /// <summary>
    /// Crée une nouvell <see cref="CantExportException"/> avec un message d'erreur spécifié.
    /// </summary>
    /// <param name="reason">Une description du problème survenu.</param>
    public CantExportException(string reason) : base(reason) { }
}