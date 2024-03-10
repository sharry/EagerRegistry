using System.Collections.Immutable;
using EagerRegistry.Generator;

namespace EagerRegistry.SourceFactories;

internal static class ServiceCollectionExtensionSourceFactory
{
	public static string CreateHintName(string assemblyName) => $"{assemblyName}.ServiceCollectionExtension.g.cs";

	public static string CreateSource(string assemblyName, ImmutableArray<RegistryCandidate> entries)
	{
		var assemblyNameDotless = assemblyName.Replace(".", "");
		return $$"""
		         {{Constants.Header}}
		         
		         #nullable enable
		         
		         using {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.Extensions;
		         
		         namespace {{assemblyName}}
		         {
		             public static class {{assemblyNameDotless}}ServiceCollectionExtensions
		             {
		                 public static {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection Add{{assemblyNameDotless}}Services(this {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection services)
		                 {
		         {{FormatEntries(entries)}}
		                    return services;
		                 }
		             }
		         }
		         """;
	}
	private static string FormatEntries(ImmutableArray<RegistryCandidate> registryEntries)
	{
		var result = string.Empty;
		foreach (var entry in registryEntries)
		{
			if (entry is null) continue;
			result += $"{FormatEntry(entry)}\n";
		}
		return result;
	}
	
	private static string FormatEntry(RegistryCandidate entry)
	{
		return entry.ImplementationTypeFqn is null
			? $"\t\t\tservices.TryAdd{entry.ServiceLifetime}<{entry.ServiceTypeFqn}>();"
			: $"\t\t\tservices.TryAdd{entry.ServiceLifetime}<{entry.ServiceTypeFqn}, {entry.ImplementationTypeFqn}>();";
	}
}