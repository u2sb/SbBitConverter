using System;
using Sb.Extensions.System;

namespace SbBitConverter.Attributes;

/// <summary>
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
public class SbBitConverterStructAttribute : Attribute
{
  /// <summary>
  /// </summary>
  /// <param name="mode"></param>
  public SbBitConverterStructAttribute(BigAndSmallEndianEncodingMode mode = BigAndSmallEndianEncodingMode.DCBA)
  {
    BigAndSmallEndianEncodingMode = mode;
  }

  /// <summary>
  ///   编码方式
  /// </summary>
  public BigAndSmallEndianEncodingMode BigAndSmallEndianEncodingMode { get; }
}