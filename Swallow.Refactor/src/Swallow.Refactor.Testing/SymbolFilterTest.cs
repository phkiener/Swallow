namespace Swallow.Refactor.Testing;

using System.Diagnostics.CodeAnalysis;
using Abstractions.Filtering;

public abstract class SymbolFilterTest : RoslynTest
{
    protected abstract ISymbolFilter SymbolFilter { get; }

    protected async Task<ISymbol> GetSymbolAsync<TSyntax>(Func<TSyntax, bool> selector, [StringSyntax("C#")] string sourceCode)
        where TSyntax : SyntaxNode
    {
        var document = AddDocument(sourceCode);
        var syntax = await document.GetSyntaxRootAsync();
        var node = syntax!.DescendantNodes().OfType<TSyntax>().First(selector);
        var semanticModel = await document.GetSemanticModelAsync();
        var symbol = semanticModel!.GetDeclaredSymbol(node);

        return symbol ?? throw new InvalidOperationException($"Could not get declared symbol from {node}.");
    }

    protected bool RunFilter(ISymbol symbol)
    {
        return SymbolFilter.Ignore(symbol);
    }
}
