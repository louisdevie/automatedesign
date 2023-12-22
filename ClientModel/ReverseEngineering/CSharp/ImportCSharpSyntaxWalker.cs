using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
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
    private const string EVENT_ENUM_NAME = "Evenement";

    [GeneratedRegex(@"Etat(\w+)")]
    private static partial Regex StateClass();
    
    [GeneratedRegex(@"Evenement\.(\w+)")]
    private static partial Regex EventCase();

    private ImportedItemsIndex index;
    private CSharpContext context;
    private Dictionary<string, List<CSharpTransition>> transitionsAssigned;

    public ImportCSharpSyntaxWalker(ImportedItemsIndex index)
    {
        this.index = index;
        this.context = new CSharpContext();
        this.transitionsAssigned = new Dictionary<string, List<CSharpTransition>>();
    }

    public override void VisitClassDeclaration(ClassDeclarationSyntax node)
    {
        string className = node.Identifier.ToString();
        string? baseClassName = node.BaseList?.Types.FirstOrDefault()?.Type.ToString();
        var classContext = CSharpContext.Class.Other;
        
        if (baseClassName == BASE_STATE_CLASS_NAME)
        {
            this.index.CreateOrUpdateState(RemoveStateClassPrefix(className));
            classContext = CSharpContext.Class.State;
        }

        this.context.PushClass(classContext, className);
        base.VisitClassDeclaration(node);
        this.context.PopClass();
    }

    public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
    {
        if (node.Identifier.ToString() == EVENT_ENUM_NAME)
        {
            foreach (EnumMemberDeclarationSyntax enumMember in node.Members)
            {
                this.index.CreateOrUpdateEnumEvent(enumMember.Identifier.ToString());
            }
        }
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        string methodName = node.Identifier.ToString();
        var methodContext = CSharpContext.Method.Other;

        if (this.context.CurrentClassKind == CSharpContext.Class.State && methodName == TRANSITION_METHOD_NAME)
        {
            methodContext = CSharpContext.Method.Transition;
        }

        this.context.PushMethod(methodContext);

        base.VisitMethodDeclaration(node);

        this.context.PopMethod();
        this.transitionsAssigned.Clear();
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
        base.VisitSwitchStatement(node);
        this.context.PopSwitch();
    }

    public override void VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
    {
        if (this.context.CurrentMethod == CSharpContext.Method.Transition)
        {
            Match match = EventCase().Match(node.Value.ToString());
            if (match.Success)
            {
                this.context.PushCaseSwitchLabel(match.Groups[1].Value);
            }
        }

        base.VisitCaseSwitchLabel(node);
    }

    public override void VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
    {
        if (this.context.CurrentMethod == CSharpContext.Method.Transition)
        {
            this.context.PushDefaultSwitchLabel();
        }
        
        base.VisitDefaultSwitchLabel(node);
    }

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        if (this.context.CurrentMethod == CSharpContext.Method.Transition)
        {
            if (TryFindDestinationState(node.Right, out string? name))
            {
                string lhs = node.Left.ToString();
                this.transitionsAssigned.TryAdd(lhs, new List<CSharpTransition>());
                this.transitionsAssigned[lhs].Add(
                    new CSharpTransition(
                        this.RemoveStateClassPrefix(this.context.CurrentClassName),
                        this.RemoveStateClassPrefix(name ?? this.context.CurrentClassName),
                        this.context.CurrentSwitchLabelKind,
                        this.context.CurrentSwitchLabelEventName
                    )
                );
            }
        }

        base.VisitAssignmentExpression(node);
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        if (this.context.CurrentMethod == CSharpContext.Method.Transition)
        {
            string valueReturned = node.Expression?.ToString() ?? "";

            if (this.context.CurrentSwitchLabelKind == CSharpTransition.Switch.Outside
                && this.transitionsAssigned.TryGetValue(valueReturned, out var transitions)
               )
            {
                foreach (CSharpTransition transition in transitions)
                {
                    transition.AddTo(this.index);
                }
            }
            else if (node.Expression != null && TryFindDestinationState(node.Expression, out string? name))
            {
                new CSharpTransition(
                        RemoveStateClassPrefix(this.context.CurrentClassName),
                        RemoveStateClassPrefix(name ?? this.context.CurrentClassName),
                        this.context.CurrentSwitchLabelKind,
                        this.context.CurrentSwitchLabelEventName
                    )
                    .AddTo(this.index);
            }
        }
    }

    private bool TryFindDestinationState(ExpressionSyntax expression, out string? name)
    {
        bool found = false;
        name = null;
        
        if (expression is ThisExpressionSyntax)
        {
            // l'état suivant est « this », on laisse à null
            found = true;
        }
        else if (expression is ObjectCreationExpressionSyntax constructor)
        {
            // l'état suivant est « new EtatQqch(...) », on récupère le nom de la classe
            name = constructor.Type.ToString();
            found = true;
        }

        return found;
    }

    [Pure]
    [return: NotNullIfNotNull(nameof(className))]
    private string? RemoveStateClassPrefix(string? className)
    {
        Match match;
        if (className != null && (match = StateClass().Match(className)).Success)
        {
            className = match.Groups[1].Value;
        }

        return className;
    }
}