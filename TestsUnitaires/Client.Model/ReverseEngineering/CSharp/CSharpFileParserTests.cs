using AutomateDesign.Client.Model.ReverseEngineering.Items;
using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.ReverseEngineering.CSharp;

public class CSharpFileParserTests
{
    [Fact]
    public void CanParse()
    {
        IFileParser cSharpParser = new CSharpFileParser();

        Assert.True(cSharpParser.CanParse("file.cs"));
        Assert.False(cSharpParser.CanParse("file.txt"));
        Assert.True(cSharpParser.CanParse("file.xaml.cs"));
        Assert.False(cSharpParser.CanParse("file.cs.j2"));
    }

    private const string STATE_CLASS =
        """
        public class EtatA : Etat
        {
            public Etat Transition(Evenement e)
            {
                Etat prochainEtat;
                switch (e)
                {
                    case Evenement.X:
                        prochainEtat = new EtatB();
            
                    default:
                        prochainEtat = this;
                }
                return prochainEtat;
            }
            
            public void Action(Evenement e)
            {
            }
        }
        """;

    [Fact]
    public void Parse()
    {
        IFileParser cSharpParser = new CSharpFileParser();
        ImportedItemsIndex index = new();

        cSharpParser.TryParse(STATE_CLASS, index);
        ImportedItem[] result = index.GetAllItems().ToArray();

        Assert.Equal(4, result.Length);
        Assert.Contains(
            result,
            item => item is ImportedState { Name: "A", Kind: StateKind.Normal }
        );
        Assert.Contains(
            result,
            item => item is ImportedState { Name: "B", Kind: StateKind.Normal }
        );
        Assert.Contains(
            result,
            item => item is ImportedEnumEvent { Name: "X" }
        );
        Assert.Contains(
            result,
            item => item is ImportedTransition
            {
                Start.Name: "A",
                End.Name: "B",
                TriggeredBy: ImportedEnumEvent { Name: "X" }
            }
        );
    }
}