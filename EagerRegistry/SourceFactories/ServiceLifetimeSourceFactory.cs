using Microsoft.CodeAnalysis;

namespace EagerRegistry.SourceFactories;

internal static class ServiceLifetimeSourceFactory
{
	private static string CreateHintName() => $"{Constants.Namespace}.ServiceLifetime.g.cs";
	private static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     #nullable disable
		     
		     namespace {{Constants.EnumsNamespace}}
		     {
		     	/// <summary>
		     	/// Specifies the lifetime of a service in an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
		     	/// </summary>
		     	public enum ServiceLifetime
		     	{
		     		/// <summary>
		     		/// Specifies that a new instance of the service will be created every time it is requested.
		     		/// </summary>
		     		Transient,
		     		/// <summary>
		     		/// Specifies that a single instance of the service will be created.
		     		/// </summary>
		     		Singleton,
		     		/// <summary>
		     		/// Specifies that a new instance of the service will be created for each scope.
		     		/// </summary>
		     		/// <remarks>
		     		/// In ASP.NET Core applications a scope is created around each server request.
		     		/// </remarks>
		     		Scoped,
		     	}
		     }
		     """;
	public static void AddServiceLifetimeSource(this IncrementalGeneratorPostInitializationContext context)
		=> context.AddSource(CreateHintName(), CreateSource());
}