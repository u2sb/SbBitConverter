using System.Collections.Immutable;
using System.Linq;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   建模结果：可能为 <see langword="null" /> 的模型 + 需要报告的诊断。
///   <para>
///     不直接返回 <c>T?</c> 的原因：存在「既不产出模型、又必须报告诊断」的情况
///     （如结构体没有任何 [FieldOffset] 字段）。诊断必须随模型一起进入增量管线，
///     才会在缓存命中时被重放。
///   </para>
/// </summary>
/// <typeparam name="T">模型类型。</typeparam>
/// <param name="Model">模型；为 <see langword="null" /> 表示不生成代码。</param>
/// <param name="Diagnostics">需要报告的诊断。</param>
internal sealed record BuildResult<T>(T? Model, EquatableArray<DiagnosticInfo> Diagnostics)
  where T : class
{
  /// <summary>构造结果。</summary>
  public static BuildResult<T> From(T? model, params DiagnosticInfo[] diagnostics)
    => new(model, new EquatableArray<DiagnosticInfo>(diagnostics.ToImmutableArray()));
}
