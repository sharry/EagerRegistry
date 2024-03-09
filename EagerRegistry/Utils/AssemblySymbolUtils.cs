using System.Linq;
using Microsoft.CodeAnalysis;

namespace EagerRegistry.Utils;

internal static class AssemblySymbolUtils
{
	public static string GetAssemblyName(this IAssemblySymbol compilationAssembly)
	{
		string? overridenName = compilationAssembly
			.GetAttributes()
			.Where(x => x.AttributeClass?.Name is "OverrideAssemblyNameAttribute")
			.Select(x => x.ConstructorArguments.FirstOrDefault().Value?.ToString())
			.FirstOrDefault();
		return overridenName ?? compilationAssembly.Name;
	}
}