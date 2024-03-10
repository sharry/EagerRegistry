using Microsoft.CodeAnalysis;

namespace EagerRegistry.SourceFactories;

internal static class ServiceAsSourceFactory
{
	private static string CreateHintName() => $"{Constants.Namespace}.ServiceAs.g.cs";
	private static string CreateSource()
		=> $$"""
		     {{Constants.Header}}
		     
		     #nullable disable
		     
		     namespace {{Constants.EnumsNamespace}}
		     {
		         [global::System.Flags]
		         internal enum ServiceAs : byte
		         {
		         	None						= 0b_000,
		         	StandaloneClass				= 0b_001,
		         	ClassWithInterface			= 0b_010,
		         	ClassWithMultipleInterfaces = 0b_100,
		         	Any							= 0b_111,
		         }
		     }
		     """;
	public static void AddServiceAsSource(this IncrementalGeneratorPostInitializationContext context)
		=> context.AddSource(CreateHintName(), CreateSource());
}