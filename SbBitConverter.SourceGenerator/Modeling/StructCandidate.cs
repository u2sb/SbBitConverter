namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   [SbBitConverterStruct] 的第一阶段候选模型（值可比较，字段尺寸以编码表示）。
/// </summary>
/// <param name="HintName">生成文件的 HintName。</param>
/// <param name="StructName">结构体名。</param>
/// <param name="NamespaceName">命名空间显示名；全局命名空间为空串。</param>
/// <param name="IsGlobalNamespace">是否全局命名空间。</param>
/// <param name="Mode">编码模式原始字节值。</param>
/// <param name="Fields">参与布局的字段候选（按成员声明顺序）。</param>
/// <param name="Location">结构体声明的可比较位置。</param>
internal sealed record StructCandidate(
  string HintName,
  string StructName,
  string NamespaceName,
  bool IsGlobalNamespace,
  byte Mode,
  EquatableArray<StructFieldCandidate> Fields,
  LocationInfo? Location);

/// <summary>
///   [SbBitConverterStruct] 的单个字段候选（值可比较）。
/// </summary>
/// <param name="Name">字段（或属性）名。</param>
/// <param name="TypeName">类型显示名。</param>
/// <param name="Offset">[FieldOffset] 偏移（字节）。</param>
/// <param name="SizeCode">尺寸编码（<see cref="Utils.GetSizeCode" />）。</param>
internal sealed record StructFieldCandidate(
  string Name,
  string TypeName,
  int Offset,
  int SizeCode);
