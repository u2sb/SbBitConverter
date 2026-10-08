namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   [SbBitConverterArray] 的第一阶段候选模型（值可比较，不含最终尺寸）。
/// </summary>
/// <param name="HintName">生成文件的 HintName。</param>
/// <param name="StructName">结构体名。</param>
/// <param name="NamespaceName">命名空间显示名；全局命名空间为空串。</param>
/// <param name="IsGlobalNamespace">是否全局命名空间。</param>
/// <param name="ElementTypeName">元素类型显示名。</param>
/// <param name="ElementSizeCode">元素尺寸编码（<see cref="Utils.GetSizeCode" />）。</param>
/// <param name="IsBaseType">元素是否为已知内置 unmanaged 类型。</param>
/// <param name="ExplicitElementSize">用户显式指定的 ElementSize；未指定为 0。</param>
/// <param name="Length">数组长度。</param>
/// <param name="Mode">编码模式原始字节值。</param>
/// <param name="HasStructLayoutAttribute">结构体是否已带 [StructLayout]。</param>
/// <param name="IsReadonlyStruct">是否 readonly struct。</param>
/// <param name="Location">结构体声明的可比较位置。</param>
internal sealed record ArrayCandidate(
  string HintName,
  string StructName,
  string NamespaceName,
  bool IsGlobalNamespace,
  string ElementTypeName,
  int ElementSizeCode,
  bool IsBaseType,
  bool IsEnumType,
  int ExplicitElementSize,
  int Length,
  byte Mode,
  bool HasStructLayoutAttribute,
  bool IsReadonlyStruct,
  LocationInfo? Location);
