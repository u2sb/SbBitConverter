using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

namespace Sb.Extensions.System;

#region 大小端枚举

// ReSharper disable InconsistentNaming
/// <summary>
///   大小端编码方式
/// </summary>
public enum BigAndSmallEndianEncodingMode : byte
{
  /// <summary>
  ///   小端模式
  /// </summary>
  DCBA = 0,

  /// <summary>
  ///   大端模式
  /// </summary>
  ABCD = 1,

  /// <summary>
  ///   前后顺序不变 二字节内部翻转
  /// </summary>
  BADC = 2,

  /// <summary>
  ///   二字节内部不变 前后顺序翻转
  /// </summary>
  CDAB = 3
}

// ReSharper restore InconsistentNaming

#endregion

/// <summary>
///   转换类
/// </summary>
public static partial class SbBitConverter
{
  /// <summary>
  ///   通用 转换
  /// </summary>
  /// <param name="source"></param>
  /// <typeparam name="T"></typeparam>
  extension<T>(scoped ref T source) where T : unmanaged
  {
    /// <summary>
    ///   解释为为 ByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> AsByteSpan()
    {
      return SpanExtension.CreateSpan(ref Unsafe.As<T, byte>(ref source), Unsafe.SizeOf<T>());
    }

    /// <summary>
    ///   解释为 ReadOnlyByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return MemoryMarshal.AsBytes(SpanExtension.CreateReadOnlySpan(in source));
    }

    /// <summary>
    ///   从字节数据读取值
    /// </summary>
    /// <param name="bs">字节数据</param>
    /// <param name="mode">大小端模式</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadFromBytes(ReadOnlySpan<byte> bs, BigAndSmallEndianEncodingMode mode)
    {
      CheckLength(bs, Unsafe.SizeOf<T>());
      source = MemoryMarshal.Read<T>(bs);
      var span = source.AsByteSpan();
      span.ApplyEndianness(mode);
    }
  }

  /// <summary>
  ///   通用 转换
  /// </summary>
  /// <param name="source"></param>
  /// <typeparam name="T"></typeparam>
  extension<T>(T source) where T : unmanaged
  {
    /// <summary>
    ///   转换到 byte[]
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] ToByteArray(bool useBigEndianMode = false)
    {
      return ToByteArray(source,
        useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
    }

    /// <summary>
    ///   转换到 byte[]
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode)
    {
      var size = Unsafe.SizeOf<T>();
      var result = new byte[size];
      WriteTo(source, result.AsSpan(), mode);
      return result;
    }

    /// <summary>
    ///   转换到 byte[]
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="mode"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteTo(scoped in Span<byte> destination, BigAndSmallEndianEncodingMode mode)
    {
      var size = Unsafe.SizeOf<T>();
      if (destination.Length < size)
        throw new ArgumentException("Destination span is too short.", nameof(destination));

#if NET8_0_OR_GREATER
      MemoryMarshal.Write(destination, in source);
#else
      MemoryMarshal.Write(destination, ref source);
#endif
      destination[..size].ApplyEndianness(mode);
    }
  }

  #region 检查长度

  /// <summary>
  ///   检查长度是否符合要求
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="expectedLength">预期长度</param>
  /// <exception cref="InvalidArrayLengthException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CheckLength(ReadOnlySpan<byte> data, int expectedLength)
  {
    if (data.Length < expectedLength) throw new InvalidArrayLengthException(expectedLength, data.Length);
  }

  /// <summary>
  ///   检查长度是否符合要求
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="expectedLength">预期长度</param>
  /// <exception cref="InvalidArrayLengthException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CheckLength(Span<byte> data, int expectedLength)
  {
    if (data.Length < expectedLength) throw new InvalidArrayLengthException(expectedLength, data.Length);
  }

  /// <summary>
  ///   检查长度是否符合要求
  /// </summary>
  /// <param name="data"></param>
  /// <param name="expectedLength"></param>
  /// <exception cref="InvalidArrayLengthException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CheckLength(byte[] data, int expectedLength)
  {
    if (data.Length < expectedLength) throw new InvalidArrayLengthException(expectedLength, data.Length);
  }

  #endregion
}

#region 长度错误异常

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

#endregion
