// using System;
//
// namespace EagerRegistry.Sample.Services;
//
//
// public interface IServiceCollection
// {
// 	IServiceCollection AddSingleton<TService, TImplementation>(TService service, TImplementation implementation);
// 	IServiceCollection AddTransient<TService, TImplementation>(TService service, TImplementation implementation);
// 	IServiceCollection AddScoped<TService, TImplementation>(TService service, TImplementation implementation);
// 	IServiceCollection AddSingleton<TService>(TService service);
// 	IServiceCollection AddTransient<TService>(TService service);
// 	IServiceCollection AddScoped<TService>(TService service);
// }
// public static class SampleServiceCollectionExtensions
// {
// 	public static IServiceCollection AddEagerRegistrySampleServices(this IServiceCollection services)
// 	{
// 		foreach (var entry in EagerRegistrySampleRegistry.Services)
// 		{
// 			if (entry.ImplementationType is null)
// 			{
// 				return entry.Lifetime switch
// 				{
// 					global::EagerRegistry.Enums.ServiceLifetime.Singleton => services.AddSingleton(entry.ServiceType),
// 					global::EagerRegistry.Enums.ServiceLifetime.Transient => services.AddTransient(entry.ServiceType),
// 					global::EagerRegistry.Enums.ServiceLifetime.Scoped => services.AddScoped(entry.ServiceType),
// 					_ => throw new ArgumentOutOfRangeException()
// 				};
// 			}
//
// 			return entry.Lifetime switch
// 			{
// 				global::EagerRegistry.Enums.ServiceLifetime.Singleton => services.AddSingleton(entry.ServiceType, entry.ImplementationType),
// 				global::EagerRegistry.Enums.ServiceLifetime.Transient => services.AddTransient(entry.ServiceType, entry.ImplementationType),
// 				global::EagerRegistry.Enums.ServiceLifetime.Scoped => services.AddScoped(entry.ServiceType, entry.ImplementationType),
// 				_ => throw new ArgumentOutOfRangeException()
// 			};
// 		}
// 		return services;
// 	}
// }