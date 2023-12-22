using System.Globalization;
using System.Numerics;
using System.Text;

namespace AutomateDesign.Client.Model.Export.Latex;

/// <summary>
/// Permets d'écrire du code LaTeX.
/// </summary>
internal class LatexFileWriter : IDisposable
{
    private class LatexEnvironment : IDisposable
    {
        private LatexFileWriter writer;
        private string name;

        public LatexEnvironment(LatexFileWriter writer, string name)
        {
            this.writer = writer;
            this.name = name;

            this.writer.underlyingStream.WriteLine(@"\begin{{{0}}}", this.name);
        }

        public void Dispose()
        {
            this.writer.underlyingStream.WriteLine(@"\end{{{0}}}", this.name);
        }
    }

    private readonly StreamWriter underlyingStream;

    /// <summary>
    /// Crée un flux en lecture permettant d'écrire du code LaTeX dans un fichier.
    /// </summary>
    /// <param name="path">Le chemin du fichier à créer ou écraser.</param>
    public LatexFileWriter(string path)
    {
        this.underlyingStream = File.CreateText(path);
    }

    public void Dispose()
    {
        this.underlyingStream.Dispose();
    }

    /// <summary>
    /// Écris un environnement latex. La balise fermante sera écrite quand l'objet renvoyé sera détruit.
    /// </summary>
    /// <example>
    /// Cette méthode s'utilise le mieux avec un using :
    /// <code>
    /// using (writer.Environment("qqch"))
    /// {
    ///   // code latex à insérer entre les balises begin et end
    /// }
    /// </code>
    /// </example>
    /// <param name="name">Le nom de l'environnement.</param>
    /// <returns>Un objet qui permets d'écrire la balise fermante quand il est détruit.</returns>
    public IDisposable Environment(string name)
    {
        return new LatexEnvironment(this, name);
    }

    /// <summary>
    /// Écris la déclaration de classe de document.
    /// </summary>
    /// <param name="cls">La classe de document à utiliser (par exemple article, report, book, ...)</param>
    public void DocumentClass(string cls)
    {
        this.underlyingStream.WriteLine(@"\documentclass{{{0}}}", cls);
    }

    /// <summary>
    /// Écris une déclaration de package.
    /// </summary>
    /// <param name="packageName">Le nom du package à utiliser.</param>
    /// <param name="options">Des options supplémentaires.</param>
    public void WriteUsePackage(string packageName, params string[] options)
    {
        this.underlyingStream.Write(@"\usepackage");
        if (options.Length > 0)
        {
            this.underlyingStream.Write("[");
            this.underlyingStream.Write(String.Join(", ", options));
            this.underlyingStream.Write("]");
        }

        this.underlyingStream.WriteLine(@"{{{0}}}", packageName);
    }

    /// <summary>
    /// Écris une balise latex générique.
    /// </summary>
    /// <param name="tagName">Le nom de la balise.</param>
    /// <param name="content">Le contenu de la balise.</param>
    public void Tag(string tagName, string content)
    {
        this.underlyingStream.WriteLine(@"\{0}{{{1}}}", tagName, content);
    }

    /// <summary>
    /// Écris un commentaire sur une seule ligne.
    /// </summary>
    /// <param name="comment">Le texte du commentaire.</param>
    public void Comment(string comment)
    {
        this.underlyingStream.WriteLine(@"% {0}", comment);
    }

    /// <summary>
    /// Écris une ligne vide.
    /// </summary>
    public void BlankLine()
    {
        this.underlyingStream.WriteLine();
    }

    /// <summary>
    /// Écris une balise de nœud Tikz.
    /// </summary>
    /// <param name="properties">Les propriétés du nœud.</param>
    /// <param name="id">L'identifiant du nœud.</param>
    /// <param name="position">La position du nœud sur le diagramme.</param>
    /// <param name="text">Le texte dans le nœud.</param>
    public void Node(string properties, string id, Vector2 position, string text)
    {
        this.underlyingStream.WriteLine(
            String.Format(
                CultureInfo.InvariantCulture,
                @"\node[{0}] ({1}) at ({2:0.00}, {3:0.00}) {{{4}}};",
                properties,
                id,
                position.X,
                position.Y,
                text
            )
        );
    }

    /// <summary>
    /// Écris une balise draw qui dessine une flèche entre deux nœuds existants avec un label.
    /// </summary>
    /// <param name="startId">L'identifiant du nœud de départ.</param>
    /// <param name="endId">L'identifiant du nœud d'arrivée.</param>
    /// <param name="labelProperties">Les propriétés du label.</param>
    /// <param name="labelText">Le contenu du label.</param>
    public void DrawArrowWithLabel(string startId, string endId, string labelProperties, string labelText)
    {
        this.underlyingStream.WriteLine(
            @"\draw[-{{>[scale=3, length=2, width=3]}}] ({0}) -- node[{2}] {{{3}}} ({1});",
            startId,
            endId,
            labelProperties,
            labelText
        );
    }
}