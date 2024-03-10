using System.Linq;
using System.Threading;
using EagerRegistry.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EagerRegistry.Generator;

internal sealed partial class EagerRegistryGenerator
{
	private static RegistryCandidate[] Transform(GeneratorSyntaxContext context, CancellationToken _)
	{
		var classDeclaration = (ClassDeclarationSyntax)context.Node;
		var classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclaration);
		if (classSymbol is not INamedTypeSymbol @class)
		{
			return [];
		}
		var attributes = @class.GetAttributes();
		
		var serviceAsValue = 0;
		var excludeAttribute = attributes.FirstOrDefault(x => x.AttributeClass?.Name is "ExcludeFromRegistryAttribute");
		if (excludeAttribute is not null)
		{
			if (!excludeAttribute.ConstructorArguments.IsEmpty)
			{
				var arg = excludeAttribute.ConstructorArguments.FirstOrDefault();
				serviceAsValue = arg.Value is byte value ? value : 7;
			}
			else
			{
				serviceAsValue = 7;
			}
		}
		
		if (serviceAsValue is 7)
		{
			return [];
		}

		var lifetime = attributes.GetServiceLifetime();
		var @interface = @class.Interfaces.FirstOrDefault();
		var interfaceFqn = @interface?.ToDisplayString();
		var classFqn = @class.ToDisplayString();
		
		if (interfaceFqn is null)
		{
			return serviceAsValue switch
			{
				1 => [],
				_ =>
				[
					new RegistryCandidate(classFqn, null, lifetime)
				]
			};
		}

		return serviceAsValue switch
		{
			1 => [new RegistryCandidate(interfaceFqn, classFqn, lifetime),],
			2 => [new RegistryCandidate(classFqn, null, lifetime),],
			_ =>
			[
				new RegistryCandidate(interfaceFqn, classFqn, lifetime),
				new RegistryCandidate(classFqn, null, lifetime)
			]
		};
	}
}