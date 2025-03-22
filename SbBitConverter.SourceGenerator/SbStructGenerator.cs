using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SbBitConverter.SourceGenerator;

[Generator]
public class SbStructGenerator : ISourceGenerator
{
  public void Initialize(GeneratorInitializationContext context)
  {
    context.RegisterForSyntaxNotifications(() => new SbBitConverterStructSyntaxReceiver());
  }


  public void Execute(GeneratorExecutionContext context)
  {
    if (context.SyntaxReceiver is not SbBitConverterStructSyntaxReceiver receiver) return;

    foreach (var structDecl in receiver.Structs)
    {
      var model = context.Compilation.GetSemanticModel(structDecl.SyntaxTree);
      if (model.GetDeclaredSymbol(structDecl) is not INamedTypeSymbol structSymbol) continue;

      SbBitConverterStructGenerator.Gen(context, structSymbol);
      SbBitConverterArrayGenerator.Gen(context, structSymbol);
    }
  }
}

internal class SbBitConverterStructSyntaxReceiver : ISyntaxReceiver
{
  public List<StructDeclarationSyntax> Structs { get; } = [];

  public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
  {
    if (syntaxNode is StructDeclarationSyntax { AttributeLists.Count: > 0 } structDecl)
      Structs.Add(structDecl);
  }
}