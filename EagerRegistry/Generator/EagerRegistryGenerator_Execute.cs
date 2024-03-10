using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EagerRegistry.SourceFactories;
using EagerRegistry.Utils;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Generator;

internal sealed partial class EagerRegistryGenerator
{
	private static void Execute(SourceProductionContext context,
		(ImmutableArray<EagerRegistryCandidate[]> Candidates,
			(Compilation Compilation, IEnumerable<ModuleInfo> Modules) Extras) capture)
	{
		var assemblyName = capture.Extras.Compilation.Assembly.GetAssemblyName();
		var assemblyAttributes = capture.Extras.Compilation.Assembly.GetAttributes();
		
		if (assemblyAttributes.HasExcludeFromRegistryAttribute()) return;
		
		if (capture.Candidates.IsEmpty) return;

		ImmutableArray<EagerRegistryCandidate> registryEntries;
		if (assemblyAttributes.HasLazyRegistryAttribute())
		{
			registryEntries = capture.Candidates
				.SelectMany(x => x)
				.Distinct()
				.Where(x => x.ServiceLifetime is not null)
				.ToImmutableArray();
			
			context.AddSource(
				RegistrySourceFactory.CreateHintName(assemblyName),
				RegistrySourceFactory.CreateSource(assemblyName, registryEntries));
		}
		else // Eager registry
		{
			var assemblyLifetime = assemblyAttributes.GetAssemblyLifetime();
			registryEntries = capture.Candidates
				.SelectMany(x => x)
				.Distinct()
				.Select(c => c with { ServiceLifetime = c.ServiceLifetime ?? assemblyLifetime })
				.ToImmutableArray();

			context.AddSource(
				RegistrySourceFactory.CreateHintName(assemblyName),
				RegistrySourceFactory.CreateSource(assemblyName, registryEntries));
		}
		
		var assemblyModulesIncludeDiExtensions = capture.Extras.Modules
			.Any(x => x.Name is "Microsoft.Extensions.DependencyInjection.dll");
		if (!assemblyModulesIncludeDiExtensions) return;
		context.AddSource(
			ServiceCollectionExtensionSourceFactory.CreateHintName(assemblyName),
			ServiceCollectionExtensionSourceFactory.CreateSource(assemblyName, registryEntries));

	}
}