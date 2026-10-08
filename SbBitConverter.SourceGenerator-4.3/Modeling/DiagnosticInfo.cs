using Microsoft.CodeAnalysis;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   一条可比较的诊断。
///   <para>
///     <see cref="DiagnosticDescriptor" /> 是 static readonly 单例，引用相等即可；
///     位置用 <see cref="LocationInfo" /> 而非 Roslyn <see cref="Location" />。
///     诊断随模型进入增量管线，缓存命中时会被重放。
///   </para>
/// </summary>
/// <param name="Descriptor">诊断描述符。</param>
/// <param name="Location">位置；<see langword="null" /> 表示无具体位置。</param>
/// <param name="MessageArgs">消息参数。</param>
internal sealed record DiagnosticInfo(
  DiagnosticDescriptor Descriptor,
  LocationInfo? Location,
  EquatableArray<string> MessageArgs)
{
  /// <summary>构造一条诊断（Roslyn 位置自动转换为可比较形式）。</summary>
  public static DiagnosticInfo Create(
    DiagnosticDescriptor descriptor, Microsoft.CodeAnalysis.Location? location, params object?[] messageArgs)
    => new(descriptor, LocationInfo.From(location), ToEquatable(messageArgs));

  /// <summary>构造一条诊断（已解码的可比较位置）。</summary>
  public static DiagnosticInfo Create(
    DiagnosticDescriptor descriptor, LocationInfo? location, params object?[] messageArgs)
    => new(descriptor, location, ToEquatable(messageArgs));

  /// <summary>还原为 Roslyn <see cref="Diagnostic" />。</summary>
  public Diagnostic ToDiagnostic()
    => Diagnostic.Create(Descriptor, Location?.ToLocation() ?? Microsoft.CodeAnalysis.Location.None,
      MessageArgs.ToArray());

  private static EquatableArray<string> ToEquatable(object?[] args)
  {
    if (args.Length == 0) return EquatableArray<string>.Empty;

    var builder = System.Collections.Immutable.ImmutableArray.CreateBuilder<string>(args.Length);
    foreach (var arg in args) builder.Add(arg?.ToString() ?? string.Empty);
    return new EquatableArray<string>(builder.MoveToImmutable());
  }
}
