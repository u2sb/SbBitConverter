using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SbBitConverter.Attributes;
using static SbBitConverter.Utils.Utils;

namespace SbBitConverter.Utils;

/// <summary>
///   转换类
/// </summary>
public static class SbBitConverter
{
  /// <summary>
  ///   转换为 short 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static short ToInt16(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadInt16BigEndian(data)
      : BinaryPrimitives.ReadInt16LittleEndian(data);
  }

  /// <summary>
  ///   转换为 ushort 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ushort ToUInt16(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadUInt16BigEndian(data)
      : BinaryPrimitives.ReadUInt16LittleEndian(data);
  }

  /// <summary>
  ///   转换为 int 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ToInt32(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadInt32BigEndian(data)
      : BinaryPrimitives.ReadInt32LittleEndian(data);
  }

  /// <summary>
  ///   转换为 uint 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static uint ToUInt32(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadUInt32BigEndian(data)
      : BinaryPrimitives.ReadUInt32LittleEndian(data);
  }

  /// <summary>
  ///   转换为long类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ToInt64(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadInt64BigEndian(data)
      : BinaryPrimitives.ReadInt64LittleEndian(data);
  }

  /// <summary>
  ///   转换为 ulong 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ulong ToUInt64(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadUInt64BigEndian(data)
      : BinaryPrimitives.ReadUInt64LittleEndian(data);
  }

  /// <summary>
  ///   转换为 float 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float ToSingle(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
#if NETSTANDARD
    return ToT<float>(data, useBigEndianMode);
#else
    return useBigEndianMode
      ? BinaryPrimitives.ReadSingleBigEndian(data)
      : BinaryPrimitives.ReadSingleLittleEndian(data);
#endif
  }

  /// <summary>
  ///   转换为 double 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ToDouble(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
#if NETSTANDARD
    return ToT<double>(data, useBigEndianMode);
#else
    return useBigEndianMode
      ? BinaryPrimitives.ReadDoubleBigEndian(data)
      : BinaryPrimitives.ReadDoubleLittleEndian(data);
#endif
  }

  #region Memory转换

  /// <summary>
  ///   Memory 转换
  /// </summary>
  /// <param name="memory"></param>
  /// <typeparam name="TFrom"></typeparam>
  /// <typeparam name="TTo"></typeparam>
  /// <returns></returns>
  public static Memory<TTo> Cast<TFrom, TTo>(this Memory<TFrom> memory)
    where TFrom : unmanaged
    where TTo : unmanaged
  {
    if (typeof(TFrom) == typeof(TTo))
      return (Memory<TTo>)(object)memory;

    return new CastMemoryManager<TFrom, TTo>(memory).Memory;
  }


  /// <summary>
  /// </summary>
  /// <typeparam name="TFrom"></typeparam>
  /// <typeparam name="TTo"></typeparam>
  /// <param name="memory"></param>
  public sealed class CastMemoryManager<TFrom, TTo>(Memory<TFrom> memory) : MemoryManager<TTo>
    where TFrom : unmanaged
    where TTo : unmanaged
  {
    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
    }

    /// <inheritdoc />
    public override Span<TTo> GetSpan()
    {
      return MemoryMarshal.Cast<TFrom, TTo>(memory.Span);
    }

    /// <inheritdoc />
    public override MemoryHandle Pin(int elementIndex = 0)
    {
      var byteOffset = elementIndex * Unsafe.SizeOf<TTo>();
      var shiftedOffset = Math.DivRem(byteOffset, Unsafe.SizeOf<TFrom>(), out var remainder);

      if (remainder != 0)
        throw new ArgumentException("The input index doesn't result in an aligned item access",
          nameof(elementIndex));

      return memory[shiftedOffset..].Pin();
    }

    /// <inheritdoc />
    public override void Unpin()
    {
    }
  }

  #endregion

  #region 通用类型转换

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="source">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToByteArray<T>(this T source, bool useBigEndianMode = false) where T : unmanaged
  {
    return ToByteArray(source,
      useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="source"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToByteArray<T>(this T source, byte mode) where T : unmanaged
  {
    return ToByteArray(source, (BigAndSmallEndianEncodingMode)mode);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="source"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToByteArray<T>(this T source, BigAndSmallEndianEncodingMode mode) where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    var result = new byte[size];
    WriteTo(source, result.AsSpan(), mode);
    return result;
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this T source, Span<byte> destination, byte mode)
    where T : unmanaged
  {
    WriteTo(source, destination, (BigAndSmallEndianEncodingMode)mode);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="source"></param>
  /// <param name="destination"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this T source, Span<byte> destination, BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(destination, size);

    Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(destination)) = source;

    ApplyEndianness(destination[..size], mode);
  }

  /// <summary>
  ///   转换到T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T ToT<T>(this ReadOnlySpan<byte> data, bool useBigEndianMode = false) where T : unmanaged
  {
    return ToT<T>(data, useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
  }

  /// <summary>
  ///   转换到T
  /// </summary>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T ToT<T>(this ReadOnlySpan<byte> data, byte mode) where T : unmanaged
  {
    return ToT<T>(data, (BigAndSmallEndianEncodingMode)mode);
  }

  /// <summary>
  ///   转换到T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <returns></returns>
  /// <exception cref="ArgumentException"></exception>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static T ToT<T>(this ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode) where T : unmanaged
  {
    T value = default;
    data.WriteTo(ref value, mode);
    return value;
  }

  /// <summary>
  ///   写入到 T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="source"></param>
  /// <param name="mode"></param>
  /// <param name="destination"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this ReadOnlySpan<byte> source, ref T destination,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(source, size);

    unsafe
    {
      fixed (T* p = &destination)
      {
        var span = new Span<byte>(p, size);
        source.CopyTo(span);
        ApplyEndianness(span, mode);
      }
    }
  }

  /// <summary>
  ///   写入到 T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="source"></param>
  /// <param name="mode"></param>
  /// <param name="destination"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this Span<byte> source, ref T destination,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(source, size);

    unsafe
    {
      fixed (T* p = &destination)
      {
        var span = new Span<byte>(p, size);
        source.CopyTo(span);
        ApplyEndianness(span, mode);
      }
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void ApplyEndianness(Span<byte> span, BigAndSmallEndianEncodingMode mode)
  {
    // 如果是单字节，也就是 byte 类型，直接返回
    var size = span.Length;
    if (size == 1) return;

    if (span.Length % 2 != 0)
      throw new ArgumentException("Data length must be even.");

    switch (mode)
    {
      // 小端序模式（DCBA）
      case BigAndSmallEndianEncodingMode.DCBA:
        if (!BitConverter.IsLittleEndian) span.Reverse();
        break;

      // 大端序模式（ABCD）
      case BigAndSmallEndianEncodingMode.ABCD:
        if (BitConverter.IsLittleEndian) span.Reverse();
        break;
      case BigAndSmallEndianEncodingMode.BADC:
        // 二字节翻转，前后不翻转
        var ushortSpan = MemoryMarshal.Cast<byte, ushort>(span);
        foreach (ref var value in ushortSpan) value = BinaryPrimitives.ReverseEndianness(value);
        if (!BitConverter.IsLittleEndian) span.Reverse();
        break;
      case BigAndSmallEndianEncodingMode.CDAB:
        // 二字节不翻转，前后翻转
        // 解释为ushort，然后整体翻转
        var us = MemoryMarshal.Cast<byte, ushort>(span);
        us.Reverse();
        if (!BitConverter.IsLittleEndian) span.Reverse();
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }
  }

  #endregion
}