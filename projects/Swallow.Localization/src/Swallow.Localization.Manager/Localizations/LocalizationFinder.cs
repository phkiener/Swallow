using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace Swallow.Localization.Manager.Localizations;

public static class LocalizationFinder
{
    public static async IAsyncEnumerable<LocalizedText> FindLocalizationsAsync(Project project)
    {
        var foundLocalizations = new HashSet<LocalizedText>();

        foreach (var symbol in await FindLocalizerSymbolsAsync(project))
        {
            var documents = project.Documents.ToImmutableHashSet();
            var usages = await SymbolFinder.FindReferencesAsync(symbol, project.Solution, documents);

            foreach (var usageLocation in usages.SelectMany(static u => u.Locations))
            {
                var syntaxRoot = await usageLocation.Document.GetSyntaxRootAsync();
                var syntaxNode = syntaxRoot?.FindNode(usageLocation.Location.SourceSpan);

                // If we're not in an indexer, we don't care - could be a nameof or other kind of reference.
                if (syntaxNode is not BracketedArgumentListSyntax argumentList)
                {
                    continue;
                }

                var usedLocalizer = await SymbolFinder.FindSymbolAtPositionAsync(usageLocation.Document, usageLocation.Location.SourceSpan.Start);
                var localizationScope = GetLocalizationScope(usedLocalizer);

                var argument = argumentList.Arguments.First().Expression;
                if (argument is LiteralExpressionSyntax { Token.Value: string literal })
                {
                    var localization = new LocalizedText(usageLocation.Document.Project.Name, localizationScope, literal);
                    if (foundLocalizations.Add(localization))
                    {
                        yield return localization;
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to determine resource for '{argument.GetType().Name}' at {usageLocation.Location}");
                }
            }
        }
    }

    private static async Task<IEnumerable<IPropertySymbol>> FindLocalizerSymbolsAsync(Project project)
    {
        var localizerTypes = await SymbolFinder.FindDeclarationsAsync(
            project: project,
            name: "IStringLocalizer",
            ignoreCase: true,
            filter: SymbolFilter.Type);

        return localizerTypes.OfType<INamedTypeSymbol>()
            .SelectMany(s => s.GetMembers().OfType<IPropertySymbol>().Where(p => p.IsIndexer));
    }

    private static string? GetLocalizationScope(ISymbol localizer)
    {
        var typeName = localizer switch
        {
            IPropertySymbol property => GetRelevantTypeArgument(property.Type),
            IParameterSymbol parameter => GetRelevantTypeArgument(parameter.Type),
            IFieldSymbol field => GetRelevantTypeArgument(field.Type),
            _ => null
        };

        return typeName is null ? null : RemovePrefix(typeName.ToDisplayString(), localizer.ContainingAssembly.Name);

        static INamedTypeSymbol? GetRelevantTypeArgument(ITypeSymbol typeSymbol)
        {
            return typeSymbol is INamedTypeSymbol { TypeArguments: [INamedTypeSymbol argument, ..] } ? argument : null;
        }

        static string RemovePrefix(string text, string prefix)
        {
            return text.StartsWith(prefix)
                ? text[(prefix.Length + 1)..] // plus one to account for the dot in-between
                : text;
        }
    }

}
