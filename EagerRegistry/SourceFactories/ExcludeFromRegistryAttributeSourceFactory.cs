namespace EagerRegistry.SourceFactories;

internal static class ExcludeFromRegistryAttributeSourceFactory
{
	public static string CreateHintName() => $"{Constants.Namespace}.ExcludeFromRegistryAttribute.g.cs";
	public static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     namespace {{Constants.Namespace}}
		     {
		         /// <summary>
		         /// An attribute that indicates that the type should be excluded from the registry.
		         /// And therefore not be registered in the DI container.
		         /// If the type is an interface, all implementations of the interface will be excluded.
		         /// </summary>
		         [{{Constants.GlobalScope}}::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Class | {{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         internal sealed class ExcludeFromRegistryAttribute : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		     }
		     """;
}