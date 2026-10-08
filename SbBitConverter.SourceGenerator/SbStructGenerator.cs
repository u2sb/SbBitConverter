using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static SbBitConverter.SourceGenerator.Modeling.ModelConstants;

namespace SbBitConverter.SourceGenerator;

/// <summary>
///   <c>[SbBitConverterArray]</c> / <c>[SbBitConverterStruct]</c> 的增量源生成器。
///   <para>
///     本类型只做管线装配；语义分析在 <c>Modeling</c>，文本生成在 <c>Emitting</c>。
///   </para>
///   <remarks>
///     增量性要点：
///     <list type="bullet">
///       <item>用 <c>ForAttributeWithMetadataName</c> 按特性全名在语法层索引，不再对每个候选做全量语义分析。</item>
///       <item>建模分两阶段：transform 产出值可比较的 Candidate（含诊断），Combine 指针宽度后
///         Complete 出最终模型 —— 绝不让 <c>GeneratorSyntaxContext</c> / <c>Compilation</c> /
///         符号进入可缓存输出，否则缓存永不命中。</item>
///       <item>平台指针宽度投影为 int 参与缓存；Platform 不变时该步骤输出 Unchanged，下游不重跑。</item>
///       <item>诊断随候选一起进入管线，缓存命中时会被重放（Roslyn 的增量输出缓存包含诊断）。</item>
///     </list>
///   </remarks>
/// </summary>
[Generator]
public sealed class SbStructGenerator : IIncrementalGenerator
{
  /// <inheritdoc />
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    // 平台指针宽度：投影为 int（可比较），替代把 Compilation 带进模型
    var pointerSize = context.CompilationProvider
      .Select(static (compilation, _) => Utils.GetPointerSize(compilation.Options.Platform))
      .WithTrackingName(PointerSizeTrackingName);

    // 第一阶段：语法层索引 + 符号解码（可比较 Candidate）
    var arrayCandidates = context.SyntaxProvider
      .ForAttributeWithMetadataName(
        ConstTable.SbBitConverterArrayAttributeName,
        static (node, _) => node is StructDeclarationSyntax,
        static (ctx, _) => Modeling.ArrayModelBuilder.TryBuild(
          (INamedTypeSymbol)ctx.TargetSymbol, ctx.Attributes[0]))
      .WithTrackingName(ArrayCandidateTrackingName);

    var structCandidates = context.SyntaxProvider
      .ForAttributeWithMetadataName(
        ConstTable.SbBitConverterStructAttributeName,
        static (node, _) => node is StructDeclarationSyntax,
        static (ctx, _) => Modeling.StructModelBuilder.TryBuild(
          (INamedTypeSymbol)ctx.TargetSymbol, ctx.Attributes[0]))
      .WithTrackingName(StructCandidateTrackingName);

    // 第二阶段：结合指针宽度补全尺寸（输出可比较的 BuildResult）
    var arrayResults = arrayCandidates
      .Combine(pointerSize)
      .Select(static (pair, _) =>
        pair.Left.Model is { } candidate
          ? Modeling.ArrayModelBuilder.Complete(candidate, pair.Right)
          : new Modeling.BuildResult<Modeling.ArrayModel>(null, pair.Left.Diagnostics))
      .WithTrackingName(ArrayModelTrackingName);

    var structResults = structCandidates
      .Combine(pointerSize)
      .Select(static (pair, _) =>
        pair.Left.Model is { } candidate
          ? Modeling.StructModelBuilder.Complete(candidate, pair.Right)
          : new Modeling.BuildResult<Modeling.StructModel>(null, pair.Left.Diagnostics))
      .WithTrackingName(StructModelTrackingName);

    // 输出
    context.RegisterSourceOutput(arrayResults, static (spc, result) =>
    {
      foreach (var diagnostic in result.Diagnostics)
      {
        spc.ReportDiagnostic(diagnostic.ToDiagnostic());
      }

      if (result.Model is not { } model)
      {
        return;
      }

      spc.AddSource(model.HintName, SourceText.From(
        Emitting.ArrayEmitter.Emit(model), System.Text.Encoding.UTF8));
    });

    context.RegisterSourceOutput(structResults, static (spc, result) =>
    {
      foreach (var diagnostic in result.Diagnostics)
      {
        spc.ReportDiagnostic(diagnostic.ToDiagnostic());
      }

      if (result.Model is not { } model)
      {
        return;
      }

      spc.AddSource(model.HintName, SourceText.From(
        Emitting.StructEmitter.Emit(model), System.Text.Encoding.UTF8));
    });
  }
}
