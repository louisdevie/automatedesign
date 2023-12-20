namespace AutomateDesign.Client.Model.Logic;

public class TextNormaliserTests
{
    [Fact]
    public void ToIdentifier()
    {
        Assert.Equal("MotDePasse", TextNormaliser.ToIdentifier(" moT De pASse "));
        Assert.Equal("LalcoolCestDeLeau", TextNormaliser.ToIdentifier("L'alcool - c'est de l'eau"));
        Assert.Equal("NoelBientot", TextNormaliser.ToIdentifier("noël bientôt"));
    }
}