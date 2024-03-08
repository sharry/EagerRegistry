﻿namespace EagerRegistry.SourceFactories;

internal static class OverrideAssemblyNameAttributeSourceFactory
{
	public static string CreateHintName() => $"{Constants.Namespace}.OverrideAssemblyNameAttributes.g.cs";
	public static string CreateSource()
		=> $$"""
		     {{Constants.Header}}

		     namespace {{Constants.Namespace}}
		     {
		         /// <summary>
		         /// An attribute that is used to override the assembly name for the generated source.
		         /// </summary>
		         [global::System.AttributeUsage({{Constants.GlobalScope}}::System.AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
		         #pragma warning disable CS9113 // Parameter is unread.
		         internal sealed class OverrideAssemblyNameAttribute(string assemblyName) : {{Constants.GlobalScope}}::System.Attribute
		         {
		         }
		         #pragma warning restore CS9113 // Parameter is unread.
		     }
		     """;
}