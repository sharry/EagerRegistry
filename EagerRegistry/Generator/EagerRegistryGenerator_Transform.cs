using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		var classFqn = @class.ToDisplayString();

		if (!@class.Interfaces.Any())
		{
			return serviceAsValue switch
			{
				1 => [],
				_ => [new (classFqn, null, lifetime)]
			};
		}

		IEnumerable<RegistryCandidate> candidates = [];
		foreach (var interfaceFqn in @class.Interfaces.Select(@interface => @interface?.ToDisplayString()))
		{
			if (interfaceFqn is null)
			{
				candidates = serviceAsValue switch
				{
					1 => candidates,
					_ => candidates.Append(new (classFqn, null, lifetime))
				};
			}
			else
			{
				candidates =  serviceAsValue switch
				{
					1 => candidates.Append(new (interfaceFqn, classFqn, lifetime)),
					2 => candidates.Append(new (classFqn, null, lifetime)),
					_ => candidates
							.Append(new (classFqn, null, lifetime))
							.Append(new (interfaceFqn, classFqn, lifetime))
				};
			}
		}

		return candidates.ToArray();
	}
}