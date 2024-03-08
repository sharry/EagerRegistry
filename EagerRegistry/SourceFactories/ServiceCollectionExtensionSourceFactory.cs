namespace EagerRegistry.SourceFactories;

internal static class ServiceCollectionExtensionSourceFactory
{
	public static string CreateHintName() => $"{Constants.Namespace}.ServiceCollectionExtension.g.cs";
	public static string CreateSource(string assemblyName)
		=> $$"""
		     {{Constants.Header}}
		     
		     #nullable enable
		     
		     using {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}};
		     
		     namespace {{assemblyName}}
		     {
		         public static class {{assemblyName.Replace(".", "")}}ServiceCollectionExtensions
		         {
		             public static {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection Add{{assemblyName.Replace(".", "")}}Services(this {{Constants.GlobalScope}}::{{Constants.ServiceCollectionNamespace}}.IServiceCollection services)
		             {
		                 foreach ({{Constants.GlobalScope}}::{{Constants.Namespace}}.RegistryEntry entry in {{Constants.GlobalScope}}::{{assemblyName}}.{{assemblyName.Replace(".", "")}}Registry.Services)
		                 {
		                 	if (entry.ImplementationType is null)
		                 	{
		                 		return entry.Lifetime switch
		                 		{
		                 			{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Singleton => services.AddSingleton(entry.ServiceType),
		                 			{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Transient => services.AddTransient(entry.ServiceType),
		                 			{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Scoped => services.AddScoped(entry.ServiceType),
		                 			_ => throw new {{Constants.GlobalScope}}::System.ArgumentOutOfRangeException()
		                 		};
		                 	}
		                 
		                 	return entry.Lifetime switch
		                 	{
		                 		{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Singleton => services.AddSingleton(entry.ServiceType, entry.ImplementationType),
		                 		{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Transient => services.AddTransient(entry.ServiceType, entry.ImplementationType),
		                 		{{Constants.GlobalScope}}::{{Constants.EnumsNamespace}}.ServiceLifetime.Scoped => services.AddScoped(entry.ServiceType, entry.ImplementationType),
		                 		_ => throw new {{Constants.GlobalScope}}::System.ArgumentOutOfRangeException()
		                 	};
		                 }
		                 return services;
		             }
		         }
		     }
		     """;
}