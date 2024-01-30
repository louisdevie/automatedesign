using AutomateDesign.Client.Model.Logic.Exceptions;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Export;

/// <summary>
/// Une chaîne de responsabilité capable d'exporter un diagramme dans un fichier.
/// </summary>
public abstract class Exporter
{
    private Exporter? other;

    /// <summary>
    /// Ajoute un <see cref="Exporter"/> à la chaîne de responsabilité.
    /// </summary>
    /// <param name="exporter"></param>
    public void Add(Exporter exporter)
    {
        if (this.other == null)
        {
            this.other = exporter;
        }
        else
        {
            this.other.Add(exporter);
        }
    }

    /// <summary>
    /// Exporte un automate dans un fichier.
    /// </summary>
    /// <param name="document">L'automate à exporter.</param>
    /// <param name="format">Le format dans lequel exporter l'automate.</param>
    /// <param name="path">Le chemin du fichier dans lequel exporter l'automate.</param>
    public void Export(Document document, ExportFormat format, string path)
    {
        if (this.CanExport(format))
        {
            this.DoExport(document, format, path);
        }
        else if (this.other != null)
        {
            this.other.Export(document, format, path);
        }
        else
        {
            throw new CantExportException($"Aucun exportateur ne prends en charge le format {format}.");
        }
    }

    /// <summary>
    /// Vérifie si un format d'exportation est supporté.
    /// </summary>
    /// <param name="format"></param>
    /// <returns></returns>
    protected abstract bool CanExport(ExportFormat format);
    
    /// <inheritdoc cref="Export"/>
    protected abstract void DoExport(Document document, ExportFormat format, string path);

    public static Exporter operator +(Exporter? lhs, Exporter rhs)
    {
        Exporter result;
        if (lhs == null)
        {
            result = rhs;
        }
        else
        {
            lhs.Add(rhs);
            result = lhs;
        }
        return result;
    }
}