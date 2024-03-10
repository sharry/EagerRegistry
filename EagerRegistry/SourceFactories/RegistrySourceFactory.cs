using System.Collections.Immutable;
using EagerRegistry.Generator;

namespace EagerRegistry.SourceFactories;

internal static class RegistrySourceFactory
{
	public static string CreateHintName(string assemblyName) => $"{assemblyName}.Registry.g.cs";
	public static string CreateSource(string assemblyName, ImmutableArray<RegistryCandidate> registryEntries)
		=> $$"""
		     {{Constants.Header}}
		     
		     #nullable enable
		     
		     namespace {{assemblyName}}
		     {
		         public static class {{assemblyName.Replace(".", "")}}Registry
		         {
		         	/// <summary>
		         	/// The registry of all services in the assembly.
		         	/// </summary>
		         	public static {{Constants.GlobalScope}}::System.Collections.Generic.IReadOnlyCollection<{{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry> Services => new {{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry[]
		         	{
		         	
		     {{FormatEntries(registryEntries)}}
		         	};
		         }
		     }
		     """;

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
		if (entry.ImplementationTypeFqn is null)
		{
			return $"\t\t\t{Constants.GlobalScope}::EagerRegistry.RegistryEntry.Create<{Constants.GlobalScope}::{entry.ServiceTypeFqn}>({Constants.GlobalScope}::{Constants.EnumsNamespace}.ServiceLifetime.{entry.ServiceLifetime}),";
		}
		return $"\t\t\t{Constants.GlobalScope}::EagerRegistry.RegistryEntry.Create<{Constants.GlobalScope}::{entry.ServiceTypeFqn}, {Constants.GlobalScope}::{entry.ImplementationTypeFqn}>({Constants.GlobalScope}::{Constants.EnumsNamespace}.ServiceLifetime.{entry.ServiceLifetime}),";
	}
}