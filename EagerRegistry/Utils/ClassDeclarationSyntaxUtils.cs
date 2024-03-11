﻿using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EagerRegistry.Utils;

internal static class ClassDeclarationSyntaxUtils
{
	public static bool IsNotStaticOrAbstract(this ClassDeclarationSyntax node)
	{
		return !node.Modifiers.Any(x => x.IsKind(SyntaxKind.StaticKeyword) || x.IsKind(SyntaxKind.AbstractKeyword));
	}
	public static bool IsNotNested(this ClassDeclarationSyntax node)
	{
		return node.Parent is not ClassDeclarationSyntax;
	}
	public static bool IsNotAutoGenerated(this ClassDeclarationSyntax node)
	{
		return !(node.SyntaxTree.FilePath.Contains(".g.") || node.SyntaxTree.FilePath.Contains(".generated."));
	}
	public static bool HasPublicNonStaticMethodsOrProperties(this ClassDeclarationSyntax node)
	{
		return node.DescendantNodes().OfType<MethodDeclarationSyntax>()
			       .Any(x => x.Modifiers.Any(y => y.IsKind(SyntaxKind.PublicKeyword))
			                 && !x.Modifiers.Any(y => y.IsKind(SyntaxKind.StaticKeyword)))
		       || node.DescendantNodes().OfType<PropertyDeclarationSyntax>()
			       .Any(x => x.Modifiers.Any(y => y.IsKind(SyntaxKind.PublicKeyword))
			                 && !x.Modifiers.Any(y => y.IsKind(SyntaxKind.StaticKeyword)));
	}

}