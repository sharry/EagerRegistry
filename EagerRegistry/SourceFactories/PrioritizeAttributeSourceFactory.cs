namespace EagerRegistry.SourceFactories;

internal static class PrioritizeAttributeSourceFactory
{
	public static string CreateHintName() => $"{Constants.Namespace}.PrioritizeAttribute.g.cs";
	public static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     namespace {{Constants.Namespace}}
		     {
		         /// <summary>
		         /// An attribute that can be used to prioritize a service in the DI container.
		         /// </summary>
		         [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		         internal sealed class PrioritizeAttribute : global::System.Attribute
		         {
		         }
		     }
		     """;
}