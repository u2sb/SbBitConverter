using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SbBitConverter.SourceGenerator;

[Generator]
public class SbStructGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    // 语法阶段只筛选带特性的结构体
    var structDeclarations = context.SyntaxProvider
      .CreateSyntaxProvider(
        (node, _) => node is StructDeclarationSyntax { AttributeLists.Count: > 0 },
        (ctx, _) => ctx
      );

    // 语义分析，判断 Attribute 类型
    var structSymbols = structDeclarations
      .Select((ctx, _) =>
      {
        var structDecl = (StructDeclarationSyntax)ctx.Node;
        var symbol = ctx.SemanticModel.GetDeclaredSymbol(structDecl);
        var compilation = ctx.SemanticModel.Compilation;
        return new
        {
          Symbol = symbol,
          Compilation = compilation
        };
      }).Where(x => x?.Symbol != null);

    // 生成代码
    context.RegisterSourceOutput(structSymbols, (spc, x) =>
    {
      var isUnsafe = x.Compilation is CSharpCompilation { Options.AllowUnsafe: true };
      var languageVersion = x.Compilation is CSharpCompilation csharpCompilation
        ? csharpCompilation.LanguageVersion
        : LanguageVersion.CSharp7;

      SbBitConverterStructGenerator.Gen(spc, x.Symbol!, isUnsafe, languageVersion, x.Compilation);
      SbBitConverterArrayGenerator.Gen(spc, x.Symbol!, isUnsafe, languageVersion, x.Compilation);
    });
  }
}