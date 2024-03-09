using Microsoft.CodeAnalysis;

namespace EagerRegistry.SourceFactories;

internal static class RegistryEntrySourceFactory
{
	private static string CreateHintName() => $"{Constants.Namespace}.RegistryEntry.g.cs";
	private static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     #nullable enable
		     
		     namespace {{Constants.Namespace}}
		     {
		     	/// <summary>
		     	/// A record that represents a service and its implementation.
		     	/// </summary>
		     	public record RegistryEntry({{Constants.GlobalScope}}::System.Type ServiceType, {{Constants.GlobalScope}}::System.Type? ImplementationType = null, {{Constants.EnumsNamespace}}.ServiceLifetime Lifetime = {{Constants.EnumsNamespace}}.ServiceLifetime.{{Constants.DefaultServiceLifetime}})
		     	{
		     		[{{Constants.GlobalScope}}::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This method is used for code generation.")]
		     		public static {{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry Create<TService, TImplementation>({{Constants.EnumsNamespace}}.ServiceLifetime lifetime = {{Constants.EnumsNamespace}}.ServiceLifetime.{{Constants.DefaultServiceLifetime}})
		     			where TService : class
		     			where TImplementation : class, TService
		     		{
		     			return new {{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry(typeof(TService), typeof(TImplementation), lifetime);
		     		}
		     		
		     		[{{Constants.GlobalScope}}::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This method is used for code generation.")]
		     		public static {{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry Create<TService>({{Constants.EnumsNamespace}}.ServiceLifetime lifetime = {{Constants.EnumsNamespace}}.ServiceLifetime.{{Constants.DefaultServiceLifetime}})
		     			where TService : class
		     		{
		     			return new {{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry(typeof(TService), Lifetime: lifetime);
		     		}
		     	}
		     }
		     """;
	public static void AddRegistryEntrySource(this IncrementalGeneratorPostInitializationContext context)
		=> context.AddSource(CreateHintName(), CreateSource());
}