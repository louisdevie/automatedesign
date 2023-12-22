using System.Numerics;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Export.Latex;

public class LatexExporterTests
{
    [Fact]
    public void Export()
    {
        string snippetTempFilePath = Path.GetTempFileName();
        string articleTempFilePath = Path.GetTempFileName();

        try
        {
            var snippetExporter = new LatexExporter();
            var articleExporter = new LatexExporter();

            var document = new Document();
            var stateA = document.CreateState("A", new Position(100, 100, 100, 100));
            var stateB = document.CreateState("B", new Position(500, 100, 100, 100));
            var stateC = document.CreateState("C", new Position(500, 500, 100, 100));
            var eventX = document.CreateEnumEvent("X");
            var eventY = document.CreateEnumEvent("Y");
            document.CreateTransition(stateA, stateB, new DefaultEvent());
            document.CreateTransition(stateA, stateC, eventX);
            document.CreateTransition(stateB, stateC, eventY);
            
            snippetExporter.Export(document, ExportFormat.LatexSnippet, snippetTempFilePath);
            articleExporter.Export(document, ExportFormat.LatexArticle, articleTempFilePath);

            string snippetText = File.ReadAllText(snippetTempFilePath);
            string articleText = File.ReadAllText(articleTempFilePath);
            
            Assert.True(snippetText.Length > 0);
            Assert.True(articleText.Length > 0);
        }
        finally
        {
            File.Delete(snippetTempFilePath);
            File.Delete(articleTempFilePath);
        }
    }
    
    [Fact]
    public void BestLabelPosition()
    {
        Vector2 north = new(0, 3),
                east = new(3, 0),
                south = new(0, -3),
                west = new(-3, 0);
        
        Assert.Equal("above", LatexExporter.BestLabelPosition(south, north));
        Assert.Equal("below", LatexExporter.BestLabelPosition(north, south));
        Assert.Equal("left", LatexExporter.BestLabelPosition(east, west));
        Assert.Equal("right", LatexExporter.BestLabelPosition(west, east));
        Assert.Equal("above right", LatexExporter.BestLabelPosition(south, east));
        Assert.Equal("above left", LatexExporter.BestLabelPosition(south, west));
        Assert.Equal("below right", LatexExporter.BestLabelPosition(north, east));
        Assert.Equal("below left", LatexExporter.BestLabelPosition(north, west));
    }
}
