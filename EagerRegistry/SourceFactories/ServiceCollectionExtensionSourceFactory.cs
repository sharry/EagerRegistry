using System.Collections.Immutable;
using EagerRegistry.Generator;
using System.Linq;

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
		         
		         using {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}};
		         using {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.Extensions;
		         
		         namespace {{assemblyName}}
		         {
		             public static class {{assemblyNameDotless}}ServiceCollectionExtensions
		             {
		                 public static {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection Add{{assemblyNameDotless}}Services(this {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection services)
		                 {
		         {{FormatEntries(entries)}}            return services;
		                 }
		             }
		         }
		         """;
	}
	private static string FormatEntries(ImmutableArray<RegistryCandidate> registryEntries)
	{
		var singletonClasses = registryEntries
			.Where(x => x.ServiceLifetime == "Singleton" && x.ImplementationTypeFqn is not null)
			.GroupBy(x => x.ImplementationTypeFqn)
			.Where(x => x.Count() > 1)
			.AsQueryable();
		var others = Enumerable
			.OfType<RegistryCandidate>(registryEntries)
			.Except(singletonClasses.SelectMany(x => x))
			.Aggregate(string.Empty, (current, entry) => current + $"{FormatEntry(entry)}\n");
		return singletonClasses
			.AsEnumerable()
			.Select(FormatMultipleInterfacesEntry)
			.Aggregate(others, (current, entry) => current + entry);
	}
	
	private static string FormatEntry(RegistryCandidate entry)
	{
		return entry.ImplementationTypeFqn is null
			? $"\t\t\tservices.TryAdd{entry.ServiceLifetime}<{entry.ServiceTypeFqn}>();"
			: $"\t\t\tservices.TryAdd{entry.ServiceLifetime}<{entry.ServiceTypeFqn}, {entry.ImplementationTypeFqn}>();";
	}
	private static string FormatMultipleInterfacesEntry(IGrouping<string?, RegistryCandidate> entry)
	{
		return entry
			.Aggregate(string.Empty, (current, registryCandidate) =>
				current + $"\t\t\tservices.TryAddSingleton<{registryCandidate.ServiceTypeFqn}>(x => x.GetRequiredService<{registryCandidate.ImplementationTypeFqn}>());\n");
	}
}