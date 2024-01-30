using AutomateDesign.Core.Documents;

namespace AutomateDesign.Client.Model.Export;

public class DocumentIndexTests
{
    [Fact]
    public void RegisterAndLookup()
    {
        var document = new Document();
        var index = new DocumentIndex();

        State stateA = document.CreateState("A");
        EnumEvent eventX = document.CreateEnumEvent("X");

        Assert.Throws<KeyNotFoundException>(() => index.LookUpStateIdentifier(stateA));
        Assert.Throws<KeyNotFoundException>(() => index.LookUpEventIdentifier(eventX));
        
        index.RegisterState(stateA);
        
        Assert.Equal("A", index.LookUpStateIdentifier(stateA));
        Assert.Throws<KeyNotFoundException>(() => index.LookUpEventIdentifier(eventX));
        
        index.RegisterEvent(eventX);
        
        Assert.Equal("A", index.LookUpStateIdentifier(stateA));
        Assert.Equal("X", index.LookUpEventIdentifier(eventX));
    }

    [Fact]
    public void NoDuplicates()
    {
        var document = new Document();
        var index = new DocumentIndex();

        const string SAME_NAME = "on teste aussi la normalisation :)";
        
        State state1 = document.CreateState(SAME_NAME);
        State state2 = document.CreateState(SAME_NAME);
        State state3 = document.CreateState(SAME_NAME);
        EnumEvent evt = document.CreateEnumEvent(SAME_NAME);
        
        index.RegisterState(state1);
        index.RegisterState(state2);
        index.RegisterState(state3);
        index.RegisterEvent(evt);
        
        Assert.Equal("OnTesteAussiLaNormalisation", index.LookUpStateIdentifier(state1));
        Assert.Equal("OnTesteAussiLaNormalisation2", index.LookUpStateIdentifier(state2));
        Assert.Equal("OnTesteAussiLaNormalisation3", index.LookUpStateIdentifier(state3));
        Assert.Equal("OnTesteAussiLaNormalisation", index.LookUpEventIdentifier(evt));
    }

    [Fact]
    public void GenerateFromDocument()
    {
        var document = new Document();
        State stateA = document.CreateState("A");
        EnumEvent eventX = document.CreateEnumEvent("X");
        
        var index = new DocumentIndex(document);
        
        Assert.Equal("A", index.LookUpStateIdentifier(stateA));
        Assert.Equal("X", index.LookUpEventIdentifier(eventX));
    }
}