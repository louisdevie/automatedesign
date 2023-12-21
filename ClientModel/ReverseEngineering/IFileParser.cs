using AutomateDesign.Client.Model.ReverseEngineering.Items;

namespace AutomateDesign.Client.Model.ReverseEngineering;

/// <summary>
/// Permets d'analyser un fichier pour trouver les éléments d'un automate contenus dedans.
/// </summary>
internal interface IFileParser
{
    /// <summary>
    /// Indique si cet analyseur prends supporte un type de fichier.
    /// </summary>
    /// <param name="fileName">Le nom/chemin du fichier.</param>
    /// <returns><see langword="true"/> si le type de fichier est supporté, sinon <see langword="false"/>.</returns>
    bool CanParse(string fileName);

    /// <summary>
    /// Analyse un fichier.
    /// </summary>
    /// <param name="code">Le contenu du fichier de code.</param>
    /// <param name="index">L'index dans lequel ajouter les éléments trouvés.</param>
    bool TryParse(string code, ImportedItemsIndex index);
}