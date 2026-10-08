using Microsoft.CodeAnalysis;

namespace SbBitConverter.SourceGenerator;

/// <summary>
///   生成器诊断描述符。
///   <para>
///     严重级别策略：除「确定产出不可用代码」的错误（Error）外，
///     其余先以 Info 级别进 AnalyzerReleases.Unshipped.md 试跑，
///     下个版本再升 Warning，避免因 TreatWarningsAsErrors 破坏下游构建。
///   </para>
/// </summary>
internal static class DiagnosticDescriptors
{
  private const string Category = "SbBitConverter.SourceGenerator";

  /// <summary>
  ///   SBBIT0001：[SbBitConverterArray] 特性参数无法解析
  ///   （元素类型不是 INamedTypeSymbol、长度/模式不是期望的基元类型）。
  /// </summary>
  public static readonly DiagnosticDescriptor ArrayAttributeInvalid = new(
    id: "SBBIT0001",
    title: "SbBitConverterArray 特性参数无法解析",
    messageFormat:
    "结构体 '{0}' 的 [SbBitConverterArray] 参数非法：第一个参数必须是基础类型（typeof(...)），"
    + "第二个参数必须是正整数字面量，第三个参数必须是 BigAndSmallEndianEncodingMode 字面量",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "特性参数必须是编译期常量，且元素类型必须能解析为命名类型符号。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0002：[SbBitConverterArray] 的 Length 或 ElementSize 非法 —— 会产出不可用的容器。
  /// </summary>
  public static readonly DiagnosticDescriptor ArraySizeInvalid = new(
    id: "SBBIT0002",
    title: "SbBitConverterArray 的长度或元素尺寸非法",
    messageFormat:
    "结构体 '{0}' 的 [SbBitConverterArray] 参数非法：Length = {1}、ElementSize = {2}；"
    + "Length 必须为正，ElementSize 无法确定或为非正值",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "Length <= 0 会产出零长度容器；ElementSize 无法确定时无法完成布局。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0003：struct 未声明为 partial，无法生成配套代码（会直接产生 CS0260）。
  /// </summary>
  public static readonly DiagnosticDescriptor StructNotPartial = new(
    id: "SBBIT0003",
    title: "标注了 SbBitConverter 特性的结构体未声明为 partial",
    messageFormat:
    "结构体 '{0}' 标注了 [SbBitConverterArray] / [SbBitConverterStruct] 但未声明为 partial，"
    + "源生成器无法附加代码；请将结构体声明为 partial",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "源生成器通过生成 partial 声明扩展结构体，非 partial 结构体无法扩展。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0004：嵌套或泛型 struct 不支持。
  /// </summary>
  public static readonly DiagnosticDescriptor StructNestedOrGeneric = new(
    id: "SBBIT0004",
    title: "嵌套或泛型结构体不受支持",
    messageFormat:
    "结构体 '{0}' 是嵌套结构体或泛型结构体，SbBitConverter 源生成器不支持，将跳过生成",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "生成的代码使用显式布局与 Unsafe API，嵌套与泛型场景未经验证。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0005：标注了 [SbBitConverterStruct] 但没有任何 [FieldOffset] 字段，不生成代码。
  /// </summary>
  public static readonly DiagnosticDescriptor NoFieldOffsetMembers = new(
    id: "SBBIT0005",
    title: "标注了 SbBitConverterStruct 但没有任何 FieldOffset 字段",
    messageFormat:
    "结构体 '{0}' 标注了 [SbBitConverterStruct] 但没有收集到任何 [FieldOffset] 字段，"
    + "因此不会生成任何代码；请为参与序列化的每个字段标注 [FieldOffset]",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Info,
    isEnabledByDefault: true,
    description: "生成器按 [FieldOffset] 收集字段并据其生成逐字段读写；没有该特性时无字段可生成。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0006：struct 字段类型尺寸无法确定，该字段会被静默跳过。
  /// </summary>
  public static readonly DiagnosticDescriptor FieldSizeUndeterminable = new(
    id: "SBBIT0006",
    title: "字段类型尺寸无法确定，该字段将被跳过",
    messageFormat:
    "结构体 '{0}' 的字段 '{1}' 类型 '{2}' 尺寸无法确定；该字段不会生成任何读写语句，"
    + "请改用定长的 unmanaged 类型或嵌套已标注的结构体",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Info,
    isEnabledByDefault: true,
    description: "定长二进制容器的字段必须是尺寸可静态确定的 unmanaged 类型。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");

  /// <summary>
  ///   SBBIT0007：同一 struct 同时标注 [SbBitConverterArray] 和 [SbBitConverterStruct]，
  ///   两个生成器会产出冲突的成员（构造函数 / ToByteArray / WriteTo 均重名）。
  /// </summary>
  public static readonly DiagnosticDescriptor BothAttributesOnSameStruct = new(
    id: "SBBIT0007",
    title: "同一结构体同时标注了 SbBitConverterArray 与 SbBitConverterStruct",
    messageFormat:
    "结构体 '{0}' 同时标注了 [SbBitConverterArray] 和 [SbBitConverterStruct]，两者会生成重名成员，"
    + "将跳过生成；请拆分为两个 struct，或只保留一个特性",
    category: Category,
    defaultSeverity: DiagnosticSeverity.Error,
    isEnabledByDefault: true,
    description: "两个生成路径产出的成员签名完全相同，同时生成会导致 CS0111 重定义错误。",
    helpLinkUri: "https://github.com/u2sb/SbBitConverter");
}
