﻿namespace EagerRegistry;

internal static class Constants
{
	public const string DefaultServiceLifetime = "Transient";
	public const string Namespace = nameof(EagerRegistry);
	public const string GlobalScope = "global";
	public const string ServiceCollectionNamespace = "Microsoft.Extensions.DependencyInjection";
	public const string EnumsNamespace = $"{Namespace}.Enums";
	public const string Header = $"""
	                               // <auto-generated />
	                               //------------------------------------------------------------------------------
	                               //     This code was generated by the {Namespace} source generator.
	                               //
	                               //     Changes to this file may cause incorrect behavior and will be lost if
	                               //     the code is regenerated.
	                               //------------------------------------------------------------------------------
	                               """;
}