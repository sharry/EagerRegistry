using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EagerRegistry.SourceFactories;
using EagerRegistry.Utils;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Generator;

internal sealed partial class EagerRegistryGenerator
{
	private static void Execute(SourceProductionContext context, (ImmutableArray<EagerRegistryCandidate[]> Candidates, (Microsoft.CodeAnalysis.Compilation Compilation, IEnumerable<ModuleInfo> Modules) Extras) capture)
	{
		var assemblyName = capture.Extras.Compilation.Assembly.GetAssemblyName();
		var assemblyAttributes = capture.Extras.Compilation.Assembly.GetAttributes();
		if (assemblyAttributes.Any(x => x.AttributeClass?.Name is "ExcludeFromRegistryAttribute")) return;
		var assemblyLifetime = assemblyAttributes.GetAssemblyLifetime();
		var assemblyModulesIncludeDiExtensions = capture.Extras.Modules
			.Any(x => x.Name is "Microsoft.Extensions.DependencyInjection.dll");

		if (capture.Candidates.IsEmpty) return;
		var reduced = capture.Candidates
			.SelectMany(x => x)
			.Distinct()
			.Select(c => c with { ServiceLifetime = c.ServiceLifetime ?? assemblyLifetime })
			.ToImmutableArray();

		context.AddSource(
			RegistrySourceFactory.CreateHintName(assemblyName),
			RegistrySourceFactory.CreateSource(assemblyName, reduced));

		if (!assemblyModulesIncludeDiExtensions) return;
		context.AddSource(
			ServiceCollectionExtensionSourceFactory.CreateHintName(),
			ServiceCollectionExtensionSourceFactory.CreateSource(assemblyName));

	}
}