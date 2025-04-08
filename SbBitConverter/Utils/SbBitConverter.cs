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
  #region 应用大小端

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte ApplyEndianness(this byte source, bool useBigEndianMode = false)
  {
    return source;
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static sbyte ApplyEndianness(this sbyte source, bool useBigEndianMode = false)
  {
    return source;
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static short ApplyEndianness(this short source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ushort ApplyEndianness(this ushort source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static int ApplyEndianness(this int source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static uint ApplyEndianness(this uint source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static long ApplyEndianness(this long source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ulong ApplyEndianness(this ulong source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float ApplyEndianness(this float source, bool useBigEndianMode = false)
  {
    var span = source.AsReadOnlyByteSpan();
    return span.ToT<float>(useBigEndianMode);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static double ApplyEndianness(this double source, bool useBigEndianMode = false)
  {
    var span = source.AsReadOnlyByteSpan();
    return span.ToT<float>(useBigEndianMode);
  }

#if NET8_0_OR_GREATER
  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Int128 ApplyEndianness(this Int128 source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }

  /// <summary>
  ///   应用大小端
  /// </summary>
  /// <param name="source"></param>
  /// <param name="useBigEndianMode"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static UInt128 ApplyEndianness(this UInt128 source, bool useBigEndianMode = false)
  {
    return BitConverter.IsLittleEndian != useBigEndianMode ? source : BinaryPrimitives.ReverseEndianness(source);
  }
#endif

  #endregion

  #region 快速解释

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
#if NET6_0_OR_GREATER
    return useBigEndianMode
      ? BinaryPrimitives.ReadSingleBigEndian(data)
      : BinaryPrimitives.ReadSingleLittleEndian(data);
#else
    return ToT<float>(data, useBigEndianMode);
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
#if NET6_0_OR_GREATER
    return useBigEndianMode
      ? BinaryPrimitives.ReadDoubleBigEndian(data)
      : BinaryPrimitives.ReadDoubleLittleEndian(data);
#else
    return ToT<double>(data, useBigEndianMode);
#endif
  }

#if NET8_0_OR_GREATER
  /// <summary>
  ///   转换为long类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Int128 ToInt128(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadInt128BigEndian(data)
      : BinaryPrimitives.ReadInt128LittleEndian(data);
  }

  /// <summary>
  ///   转换为 ulong 类型
  /// </summary>
  /// <param name="data">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static UInt128 ToUInt128(ReadOnlySpan<byte> data, bool useBigEndianMode = false)
  {
    return useBigEndianMode
      ? BinaryPrimitives.ReadUInt128BigEndian(data)
      : BinaryPrimitives.ReadUInt128LittleEndian(data);
  }

#endif

  #endregion

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
  private sealed class CastMemoryManager<TFrom, TTo>(Memory<TFrom> memory) : MemoryManager<TTo>
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
  ///   解释为为 ByteSpan
  /// </summary>
  /// <param name="source"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Span<byte> AsByteSpan<T>(this T source) where T : unmanaged
  {
    return MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref source), Unsafe.SizeOf<T>());
  }

  /// <summary>
  ///   解释为为 ByteSpan
  /// </summary>
  /// <param name="source"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static ReadOnlySpan<byte> AsReadOnlyByteSpan<T>(this T source) where T : unmanaged
  {
    return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref source), Unsafe.SizeOf<T>());
  }

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

    // 小端模式 初始为 DCBA
    if (BitConverter.IsLittleEndian)
      switch (mode)
      {
        case BigAndSmallEndianEncodingMode.DCBA:
          break;
        case BigAndSmallEndianEncodingMode.ABCD:
          span.Reverse();
          break;

        // 二字节翻转，前后不翻转 DCBA -> BADC
        case BigAndSmallEndianEncodingMode.BADC:
          // 解释为ushort，然后整体翻转
          var us = MemoryMarshal.Cast<byte, ushort>(span);
          us.Reverse();
          break;

        // 二字节不翻转，前后翻转 DCBA -> CDAB
        case BigAndSmallEndianEncodingMode.CDAB:
          var ushortSpan = MemoryMarshal.Cast<byte, ushort>(span);
          foreach (ref var value in ushortSpan) value = BinaryPrimitives.ReverseEndianness(value);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
    // 大端模式 初始为 ABCD
    else
      switch (mode)
      {
        case BigAndSmallEndianEncodingMode.DCBA:
          span.Reverse();
          break;
        case BigAndSmallEndianEncodingMode.ABCD:
          break;

        // 二字节翻转，前后不翻转 ABCD -> BADC
        case BigAndSmallEndianEncodingMode.BADC:
          var ushortSpan = MemoryMarshal.Cast<byte, ushort>(span);
          foreach (ref var value in ushortSpan) value = BinaryPrimitives.ReverseEndianness(value);
          break;

        // 二字节不翻转，前后翻转 ABCD -> CDAB
        case BigAndSmallEndianEncodingMode.CDAB:
          // 解释为ushort，然后整体翻转
          var us = MemoryMarshal.Cast<byte, ushort>(span);
          us.Reverse();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
  }

  #endregion
}