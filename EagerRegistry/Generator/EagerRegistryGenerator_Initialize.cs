using System.Linq;
using EagerRegistry.SourceFactories;
using EagerRegistry.Utils;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Generator;

internal record EagerRegistryCandidate(
	string ServiceTypeFqn,
	string? ImplementationTypeFqn = null,
	string? ServiceLifetime = null);

[Generator(LanguageNames.CSharp)]
internal sealed partial class EagerRegistryGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(ctx =>
		{
			ctx.AddExcludeFromRegistryAttributeSource();
			ctx.AddRegistryEntrySource();
			ctx.AddServiceLifetimeSource();
			ctx.AddServiceLifetimeAttributesSource();
			ctx.AddOverrideAssemblyNameAttributeSource();
			ctx.AddLazyRegistryAttributeSource();
		});
		var metadataProvider = context.MetadataReferencesProvider
			.Select((x, _) => x.GetModules())
			.Collect()
			.Select((x, _) => x
				.SelectMany(y => y)
				.Distinct());
		var extrasProvider = context.CompilationProvider
			.Combine(metadataProvider);
		var provider = context.SyntaxProvider
			.CreateSyntaxProvider(Filter, Transform)
			.Where(x => x.Any())
			.Collect()
			.Combine(extrasProvider);
		context.RegisterSourceOutput(provider, Execute);
	}
}
