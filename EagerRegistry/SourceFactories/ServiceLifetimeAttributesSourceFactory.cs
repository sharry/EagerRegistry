namespace EagerRegistry.SourceFactories;

internal static class ServiceLifetimeAttributesSourceFactory
{
	public static string CreateHintName() => $"{Constants.Namespace}.ServiceLifetimeAttributes.g.cs";
	public static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     namespace {{Constants.Namespace}}
		     {
		         /// <summary>
		         /// Specifies that a new instance of the service will be created every time it is requested.
		         /// </summary>
		         [{{Constants.GlobalScope}}::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Class | {{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         internal sealed class TransientAttribute : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		         /// <summary>
		         /// Specifies that a new instance of the service will be created for each scope.
		         /// </summary>
		         /// <remarks>
		         /// In ASP.NET Core applications a scope is created around each server request.
		         /// </remarks>
		         [{{Constants.GlobalScope}}::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Class | {{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         internal sealed class ScopedAttribute : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		         /// <summary>
		         /// Specifies that a single instance of the service will be created.
		         /// </summary>
		         [{{Constants.GlobalScope}}::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Class | {{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         internal sealed class SingletonAttribute : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		     }
		     """;
}