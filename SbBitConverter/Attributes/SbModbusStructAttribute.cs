using System;
using SbBitConverter.Models;

namespace SbBitConverter.Attributes;

/// <summary>
/// </summary>
/// <param name="mode"></param>
[AttributeUsage(AttributeTargets.Struct)]
public class SbBitConverterStructAttribute(BigAndSmallEndianEncodingMode mode = BigAndSmallEndianEncodingMode.DCBA)
  : Attribute
{
  /// <summary>
  ///   编码方式
  /// </summary>
  public BigAndSmallEndianEncodingMode BigAndSmallEndianEncodingMode { get; } = mode;
}