using Microsoft.CodeAnalysis;

namespace EagerRegistry.SourceFactories;

internal static class LazyRegistryAttributeSourceFactory
{
	private static string CreateHintName() => $"{Constants.Namespace}.LazyRegistryAttribute.g.cs";
	private static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     namespace {{Constants.Namespace}}
		     {
		         /// <summary>
		         /// Specifies that the assembly services should be registered lazily.
		         /// Only the services marked with a service lifetime attribute will be registered.
		         /// </summary>
		         [{{Constants.GlobalScope}}::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         internal sealed class LazyRegistryAttribute : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		     }
		     """;
	public static void AddLazyRegistryAttributeSource(this IncrementalGeneratorPostInitializationContext context)
		=> context.AddSource(CreateHintName(), CreateSource());
}