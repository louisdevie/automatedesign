using System.ComponentModel;
using System.Numerics;
using System.Security.Cryptography;
using AutomateDesign.Client.Model.Logic;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Export.Latex;

/// <summary>
/// Permets d'exporter des automates au format TikZ/LaTeX
/// </summary>
public class LatexExporter : Exporter
{
    protected override bool CanExport(ExportFormat format)
    {
        return format is ExportFormat.LatexArticle or ExportFormat.LatexSnippet;
    }

    protected override void DoExport(Document document, ExportFormat format, string path)
    {
        using LatexFileWriter writer = new(path);

        switch (format)
        {
        case ExportFormat.LatexSnippet:
            GenerateSnippetTips(writer);
            GenerateDiagram(writer, document);
            break;

        case ExportFormat.LatexArticle:
            GenerateArticlePreamble(writer);
            using (writer.Environment("document"))
            {
                GenerateDiagram(writer, document);
            }

            break;
        }
    }

    private static void GenerateArticlePreamble(LatexFileWriter writer)
    {
        writer.DocumentClass("article");
        writer.WriteUsePackage("geometry", "a4paper", "landscape");
        GenerateTikzImport(writer);
    }

    private static void GenerateDiagram(LatexFileWriter writer, Document document)
    {
        const string DEFAULT_STATE_NODE_PROPERTIES =
            "circle, draw=black, fill=white, minimum size=2cm, text width=2cm, align=center";
        const string DEFAULT_EVENT_NODE_PROPERTIES = "fill=white, pos=0, ";

        var index = new DocumentIndex(document);
        var statePositions = new Dictionary<int, Vector2>();

        using (writer.Environment("tikzpicture"))
        {
            foreach (State state in document.States)
            {
                Vector2 position = ToDocumentCoordinates(state.Position.Center);
                statePositions.Add(state.Id, position);

                writer.Node(
                    properties: DEFAULT_STATE_NODE_PROPERTIES,
                    id: index.LookUpStateIdentifier(state),
                    position,
                    text: state.Name
                );
            }

            foreach (Transition transition in document.Transitions)
            {
                writer.DrawArrowWithLabel(
                    startId: index.LookUpStateIdentifier(transition.Start),
                    endId: index.LookUpStateIdentifier(transition.End),
                    labelProperties: DEFAULT_EVENT_NODE_PROPERTIES + BestLabelPosition(
                        statePositions[transition.Start.Id],
                        statePositions[transition.End.Id]
                    ),
                    labelText: transition.TriggeredBy.DisplayName
                );
            }
        }
    }

    private static readonly string[] cardinalDirections = new string[8]
    {
        "below left",
        "below",
        "below right",
        "right",
        "above right",
        "above",
        "above left",
        "left"
    };

    internal static string BestLabelPosition(Vector2 start, Vector2 end)
    {
        const double EIGHTH_OF_CIRCLE = 0.7853981634; // un huitième d'un cercle en radians
        const double BELOW_LEFT_MINIMUM = -2.7488935719; // le plus petit angle dans le secteur sud-ouest

        Vector2 vec = end - start;
        double angle = Math.Atan2(vec.Y, vec.X);
        double
            angleFromBelowLeft =
                angle - BELOW_LEFT_MINIMUM; // l'angle à partir du point de départ des directions cardinales

        int directionIndex = (int)Math.Floor(angleFromBelowLeft / EIGHTH_OF_CIRCLE);
        return cardinalDirections[directionIndex];
    }

    private static Vector2 ToDocumentCoordinates(Vector2 vector)
    {
        // résolution d'impression en pixels par cm
        const float PPCM = 46;

        // on inverse l'axe Y (sur l'écran, il est vers le bas, sur la feuille il est vers le haut)
        return new Vector2(vector.X / PPCM, -vector.Y / PPCM);
    }

    private static void GenerateSnippetTips(LatexFileWriter writer)
    {
        writer.Comment("à inclure dans le préambule de votre document");
        GenerateTikzImport(writer);
        writer.BlankLine();

        writer.Comment("le diagramme de l'automate");
    }

    private static void GenerateTikzImport(LatexFileWriter writer)
    {
        writer.WriteUsePackage("tikz");
        writer.Tag("usetikzlibrary", "{arrows.meta}");
    }
}