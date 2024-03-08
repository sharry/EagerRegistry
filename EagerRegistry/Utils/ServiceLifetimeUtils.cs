using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Utils;

internal static class ServiceLifetimeUtils
{
	public static string? GetServiceLifetime(this ImmutableArray<AttributeData> attributes)
	{
		return attributes
			.SkipWhile(x =>
				x.AttributeClass?.Name
					is not "TransientAttribute"
					and not "ScopedAttribute"
					and not "SingletonAttribute")
			.Select(x =>
			{
				var name = x.AttributeClass?.Name;
				return name?.Remove(name.Length - "Attribute".Length);
			})
			.FirstOrDefault();
	}

	public static string GetAssemblyLifetime(this ImmutableArray<AttributeData> attributes, string defaultLifetime = Constants.DefaultServiceLifetime)
	{
		return attributes
			.SkipWhile(x =>
				x.AttributeClass?.Name
					is not "TransientAttribute"
					and not "ScopedAttribute"
					and not "SingletonAttribute")
			.Select(x =>
			{
				var name = x.AttributeClass?.Name;
				return name?.Remove(name.Length - "Attribute".Length) ?? defaultLifetime;
			})
			.DefaultIfEmpty(defaultLifetime)
			.FirstOrDefault() ?? defaultLifetime;
	}
}