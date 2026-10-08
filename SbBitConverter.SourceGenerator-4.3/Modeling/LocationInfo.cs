using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   值可比较的位置信息。
///   <para>
///     ⚠️ 不能把 Roslyn 的 <see cref="Location" /> 直接放进增量模型 ——
///     它每次编译都会产生新实例，会让增量缓存永不命中。
///     本类型只保留文件路径 + 纯值类型 span，可安全参与 record 相等性。
///   </para>
/// </summary>
/// <param name="FilePath">源文件路径。</param>
/// <param name="TextSpan">文本区间。</param>
/// <param name="LineSpan">行列区间。</param>
internal readonly record struct LocationInfo(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan)
{
  /// <summary>还原为 Roslyn <see cref="Location" />，用于报告诊断。</summary>
  public Location ToLocation() => Location.Create(FilePath, TextSpan, LineSpan);

  /// <summary>从 Roslyn 位置提取可比较形式；非源文件位置返回 <see langword="null" />。</summary>
  public static LocationInfo? From(Location? location)
    => location is null || location.Kind != LocationKind.SourceFile
      ? null
      : new LocationInfo(
        location.SourceTree?.FilePath ?? string.Empty,
        location.SourceSpan,
        location.GetLineSpan().Span);
}
