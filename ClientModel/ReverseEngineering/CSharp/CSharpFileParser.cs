using AutomateDesign.Client.Model.ReverseEngineering.Items;
using Microsoft.CodeAnalysis.CSharp;

namespace AutomateDesign.Client.Model.ReverseEngineering.CSharp;

internal class CSharpFileParser : IFileParser
{
    private const string CSHARP_FILE_EXTENSION = ".cs"; 
    
    public bool CanParse(string fileName)
    {
        return Path.GetExtension(fileName) == CSHARP_FILE_EXTENSION;
    }

    public bool TryParse(string code, ImportedItemsIndex index)
    {
        bool success = true;
        try
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var visitor = new ImportCSharpSyntaxWalker(index);

            visitor.Visit(syntaxTree.GetRoot());
        }
        catch (Exception)
        {
            success = false;
        }

        return success;
    }
}