using System.Linq;
using System.Threading;
using EagerRegistry.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EagerRegistry.Generator;

internal sealed partial class EagerRegistryGenerator
{
	private static EagerRegistryCandidate[] Transform(GeneratorSyntaxContext context, CancellationToken _)
	{
		var classDeclaration = (ClassDeclarationSyntax)context.Node;
		var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
		if (classSymbol is not INamedTypeSymbol @class)
		{
			return [];
		}
		var attributes = @class.GetAttributes();
		if (attributes.Any(x => x.AttributeClass?.Name is "ExcludeFromRegistryAttribute"))
		{
			return [];
		}

		var lifetime = attributes.GetServiceLifetime();
		var @interface = @class.Interfaces.FirstOrDefault();
		var interfaceFqn = @interface?.ToDisplayString();
		var classFqn = @class.ToDisplayString();
		if (interfaceFqn is null)
		{
			return [
				new EagerRegistryCandidate(
					classFqn,
					null,
					lifetime)
			];
		}
		return [
			new EagerRegistryCandidate(
				interfaceFqn, 
				classFqn, 
				lifetime),
			new EagerRegistryCandidate(
				classFqn,
				null,
				lifetime)
		];
	}
}