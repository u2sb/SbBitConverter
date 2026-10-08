namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   [SbBitConverterStruct] 结构体的单个字段（值可比较）。
/// </summary>
/// <param name="Name">字段（或属性）名。</param>
/// <param name="TypeName">类型显示名。</param>
/// <param name="Offset">[FieldOffset] 偏移（字节）。</param>
/// <param name="Size">类型尺寸（字节）；无法确定时为 0，生成器按嵌套类型处理。</param>
internal sealed record StructFieldModel(
  string Name,
  string TypeName,
  int Offset,
  int Size);

/// <summary>
///   [SbBitConverterStruct] 的值可比较模型。
/// </summary>
/// <param name="HintName">生成文件的 HintName。</param>
/// <param name="StructName">结构体名。</param>
/// <param name="NamespaceName">命名空间显示名；全局命名空间为空串。</param>
/// <param name="IsGlobalNamespace">是否全局命名空间。</param>
/// <param name="Mode">编码模式（<see cref="BigAndSmallEndianEncodingMode" /> 的原始字节值）。</param>
/// <param name="Fields">参与布局的字段列表（按成员声明顺序）。</param>
internal sealed record StructModel(
  string HintName,
  string StructName,
  string NamespaceName,
  bool IsGlobalNamespace,
  byte Mode,
  EquatableArray<StructFieldModel> Fields);
