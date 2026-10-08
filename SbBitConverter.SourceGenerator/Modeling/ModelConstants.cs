namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>增量管线各阶段的 TrackingName 常量（供 /reportanalyzer 观测）。</summary>
internal static class ModelConstants
{
  /// <summary>平台指针宽度投影阶段。</summary>
  public const string PointerSizeTrackingName = "SbBitConverter.PointerSize";

  /// <summary>[SbBitConverterArray] 符号解码阶段（Candidate）。</summary>
  public const string ArrayCandidateTrackingName = "SbBitConverter.ArrayCandidate";

  /// <summary>[SbBitConverterStruct] 符号解码阶段（Candidate）。</summary>
  public const string StructCandidateTrackingName = "SbBitConverter.StructCandidate";

  /// <summary>[SbBitConverterArray] 建模阶段（Complete）。</summary>
  public const string ArrayModelTrackingName = "SbBitConverter.ArrayModel";

  /// <summary>[SbBitConverterStruct] 建模阶段（Complete）。</summary>
  public const string StructModelTrackingName = "SbBitConverter.StructModel";

  /// <summary>文件名后缀：数组生成。</summary>
  public const string ArrayHintSuffix = "_SbBitConverterArray.g.cs";

  /// <summary>文件名后缀：结构体生成。</summary>
  public const string StructHintSuffix = "_SbBitConverterStruct.g.cs";
}
