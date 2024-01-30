using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace AutomateDesign.Client.Model.Logic;

/// <summary>
/// Fournit des méthodes pour normaliser du texte.
/// </summary>
public static partial class TextNormaliser
{
    [GeneratedRegex("[^A-Za-z]*")]
    private static partial Regex NonLatinAlpha();
    
    /// <summary>
    /// Transforme le nom d'un élément de manière à ce qu'il puisse être utilisé comme identifiant C# par exemple.
    /// </summary>
    /// <param name="name">Le nom à convertir.</param>
    /// <returns>Le nom sans espaces ni accent et en CassePascal.</returns>
    public static string ToIdentifier(string name)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        name = name.Normalize(NormalizationForm.FormD);
        name = textInfo.ToTitleCase(name);
        name = NonLatinAlpha().Replace(name, "");
        return name;
    }
}