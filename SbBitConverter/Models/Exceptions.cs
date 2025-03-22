using System;

namespace SbBitConverter.Models;

/// <summary>
///   数组长度和预期不一致错误
/// </summary>
/// <param name="expectedLength">预期长度</param>
/// <param name="actualLength">真实长度</param>
public class InvalidArrayLengthException(int expectedLength, int actualLength)
  : Exception($"Invalid array length. Expected: {expectedLength}, Actual: {actualLength}")
{
  /// <summary>
  ///   预期长度
  /// </summary>
  public int ExpectedLength { get; } = expectedLength;

  /// <summary>
  ///   真实长度
  /// </summary>
  public int ActualLength { get; } = actualLength;
}