using System.Collections.Immutable;
using EagerRegistry.Generator;

namespace EagerRegistry.SourceFactories;

internal static class RegistrySourceFactory
{
	public static string CreateHintName(string assemblyName) => $"{assemblyName}.Registry.g.cs";
	public static string CreateSource(string assemblyName, ImmutableArray<EagerRegistryCandidate> candidates)
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
		         	
		     {{FormatCandidates(candidates)}}
		         	};
		         }
		     }
		     """;

	private static string FormatCandidates(ImmutableArray<EagerRegistryCandidate> candidates)
	{
		var result = string.Empty;
		foreach (var candidate in candidates)
		{
			if (candidate is null) continue;
			result += $"{FormatCandidate(candidate)}\n";
		}
		return result;
	}
	
	private static string FormatCandidate(EagerRegistryCandidate candidate)
	{
		if (candidate.ImplementationTypeFqn is null)
		{
			return $"\t\t\t{Constants.GlobalScope}::EagerRegistry.RegistryEntry.Create<{Constants.GlobalScope}::{candidate.ServiceTypeFqn}>({Constants.GlobalScope}::{Constants.EnumsNamespace}.ServiceLifetime.{candidate.ServiceLifetime}),";
		}
		return $"\t\t\t{Constants.GlobalScope}::EagerRegistry.RegistryEntry.Create<{Constants.GlobalScope}::{candidate.ServiceTypeFqn}, {Constants.GlobalScope}::{candidate.ImplementationTypeFqn}>({Constants.GlobalScope}::{Constants.EnumsNamespace}.ServiceLifetime.{candidate.ServiceLifetime}),";
	}
}