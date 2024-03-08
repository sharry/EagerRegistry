using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Utils;

internal record ModuleInfo(string Name, Version Version);
internal static class MetadataReferenceUtils
{
	public static IncrementalValuesProvider<MetadataReference> GetMetadataReferencesProvider(
		this IncrementalGeneratorInitializationContext context)
	{
		var metadataProviderProperty = context.GetType()
			                               .GetProperty(nameof(context.MetadataReferencesProvider))
		                               ?? throw new Exception($"The property '{nameof(context.MetadataReferencesProvider)}' not found");

		var metadataProvider = metadataProviderProperty.GetValue(context);

		if (metadataProvider is IncrementalValuesProvider<MetadataReference> metadataValuesProvider)
			return metadataValuesProvider;

		if (metadataProvider is IncrementalValueProvider<MetadataReference> metadataValueProvider)
			return metadataValueProvider.SelectMany(static (reference, _) => ImmutableArray.Create(reference));

		throw new Exception($"The '{nameof(context.MetadataReferencesProvider)}' is neither an 'IncrementalValuesProvider<{nameof(MetadataReference)}>' nor an 'IncrementalValueProvider<{nameof(MetadataReference)}>.'");
	}
	public static IEnumerable<ModuleInfo> GetModules(this MetadataReference metadataReference)
	{
		// Project reference (ISymbol)
		if (metadataReference is CompilationReference compilationReference)
		{
			return compilationReference.Compilation.Assembly.Modules
				.Select(m => new ModuleInfo(
					m.Name,
					compilationReference.Compilation.Assembly.Identity.Version));
		}

		// DLL
		if (metadataReference is PortableExecutableReference portable
		    && portable.GetMetadata() is AssemblyMetadata assemblyMetadata)
		{
			return assemblyMetadata.GetModules()
				.Select(m => new ModuleInfo(
					m.Name,
					m.GetMetadataReader().GetAssemblyDefinition().Version));
		}

		return Array.Empty<ModuleInfo>();
	}
}