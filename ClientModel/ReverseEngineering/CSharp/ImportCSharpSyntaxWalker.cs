using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutomateDesign.Client.Model.ReverseEngineering.CSharp;

/// <summary>
/// Visite un arbre syntaxique C# pour déterminer les éléments à importer.
/// </summary>
internal partial class ImportCSharpSyntaxWalker : CSharpSyntaxWalker
{
    private const string BASE_STATE_CLASS_NAME = "Etat";
    private const string TRANSITION_METHOD_NAME = "Transition";
    private const string THIS_KEYWORD = "this";

    [GeneratedRegex(@"Evenement\.(\w+)")]
    private static partial Regex EventCase();
    
    private ImportedItemsIndex index;
    private CSharpContext context;

    public ImportCSharpSyntaxWalker(ImportedItemsIndex index)
    {
        this.index = index;
        this.context = new CSharpContext();
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        string className = node.Identifier.ToString();
        string? baseClassName = node.BaseList?.Types.FirstOrDefault()?.Type.ToString();
        var classContext = CSharpContext.Class.Other;
        
        if (baseClassName == BASE_STATE_CLASS_NAME)
        {
            this.index.CreateOrUpdateState(className);
            classContext = CSharpContext.Class.State;
        }

        this.context.PushClass(classContext);
        base.VisitClassDeclaration(node);
        this.context.PopClass();
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        string methodName = node.Identifier.ToString();
        var methodContext = CSharpContext.Method.Other;

        if (this.context.CurrentClass == CSharpContext.Class.State && methodName == TRANSITION_METHOD_NAME)
        {
            methodContext = CSharpContext.Method.Transition;
        }

        this.context.PushMethod(methodContext);
        base.VisitMethodDeclaration(node);
        this.context.PopMethod();
    }

    public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
    {
        if (this.context.CurrentMethod == CSharpContext.Method.Transition)
        {
            Match match = EventCase().Match(node.Value.ToString());
            if (match.Success)
            {
                this.context.PushCaseSwitchLabel(match.Captures[0].Value);
            }
        }
        
        base.VisitCaseSwitchLabel(node);
    }
}