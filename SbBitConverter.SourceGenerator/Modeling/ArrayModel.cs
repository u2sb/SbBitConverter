namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   [SbBitConverterArray] 的值可比较模型。所有符号级信息已在 Builder 中解码为基元值。
/// </summary>
/// <param name="HintName">生成文件的 HintName。</param>
/// <param name="StructName">结构体名。</param>
/// <param name="NamespaceName">命名空间显示名；全局命名空间为空串。</param>
/// <param name="IsGlobalNamespace">是否全局命名空间。</param>
/// <param name="ElementTypeName">元素类型显示名。</param>
/// <param name="ElementSize">元素尺寸（字节）。</param>
/// <param name="Length">数组长度。</param>
/// <param name="Mode">编码模式（<see cref="BigAndSmallEndianEncodingMode" /> 的原始字节值）。</param>
/// <param name="HasStructLayoutAttribute">结构体是否已带 [StructLayout]。</param>
/// <param name="IsBaseType">元素是否为已知内置 unmanaged 类型。</param>
/// <param name="IsEnumType">元素是否为枚举类型。</param>
/// <param name="IsReadonlyStruct">是否 readonly struct。</param>
internal sealed record ArrayModel(
  string HintName,
  string StructName,
  string NamespaceName,
  bool IsGlobalNamespace,
  string ElementTypeName,
  int ElementSize,
  int Length,
  byte Mode,
  bool HasStructLayoutAttribute,
  bool IsBaseType,
  bool IsEnumType,
  bool IsReadonlyStruct);
