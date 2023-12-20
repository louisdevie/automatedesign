namespace AutomateDesign.Client.Model.Export;

/// <summary>
/// Une énumération des différents formats d'exportation.
/// </summary>
public enum ExportFormat
{
    /// <summary>
    /// Génère une image au format PNG avec un fond transparent.
    /// </summary>
    PngImage,
    
    /// <summary>
    /// Génère une image au format JPEG avec un fond blanc.
    /// </summary>
    JpegImage,
    
    /// <summary>
    /// Génère seulement un fragment de code à insérer dans un document LaTeX existant.
    /// </summary>
    LatexSnippet,
    
    /// <summary>
    /// Génère un article LaTeX complet contenant l'automate.  
    /// </summary>
    LatexArticle,
    
    /// <summary>
    /// Génère un squelette de code C#.
    /// </summary>
    CSharpCodeTemplate,
}