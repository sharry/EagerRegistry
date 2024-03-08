using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EagerRegistry.Analysers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class MultipleLifetimeAttributesAnalyzer : DiagnosticAnalyzer
{
	public override void Initialize(AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze);
		context.EnableConcurrentExecution();
		context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
	}
	
	private static void AnalyzeSymbol(SymbolAnalysisContext context)
	{
		var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

		var lifetimeAttributes = new[] { "TransientAttribute", "ScopedAttribute", "SingletonAttribute" };
		if (namedTypeSymbol.GetAttributes().Count(a => lifetimeAttributes.Contains(a.AttributeClass?.Name)) <= 1) return;
		var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
		context.ReportDiagnostic(diagnostic);
	}

	private static readonly DiagnosticDescriptor Rule = new(
		"ER001",
		"Multiple lifetime attributes applied",
		"Only one of Transient, Scoped, or Singleton can be applied to a class",
		"Design",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true);
	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);
}