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

  #region Memory 转换

  /// <summary>
  ///   解释为 MemoryByte
  /// </summary>
  /// <param name="data"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static Memory<byte> AsByteMemory<T>(this Memory<T> data) where T : struct
  {
    return Cast<T, byte>(data);
  }

  /// <summary>
  ///   Memory转换 类型解释
  /// </summary>
  /// <param name="data"></param>
  /// <typeparam name="TFrom"></typeparam>
  /// <typeparam name="TTo"></typeparam>
  /// <returns></returns>
  public static Memory<TTo> Cast<TFrom, TTo>(this Memory<TFrom> data) where TFrom : struct where TTo : struct
  {
    using var cmm = new CastMemoryManager<TFrom, TTo>(data);
    return cmm.Memory;
  }

  /// <summary>
  ///   转换到Memory
  /// </summary>
  /// <param name="data"></param>
  /// <typeparam name="TFrom"></typeparam>
  /// <typeparam name="TTo"></typeparam>
  /// <returns></returns>
  public static Memory<TTo> AsMemory<TFrom, TTo>(this ReadOnlySpan<TFrom> data)
    where TFrom : struct where TTo : struct
  {
    unsafe
    {
      var ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(data));
      using var cmm = new PointerMemoryManager<TTo>(ptr, data.Length * Unsafe.SizeOf<TFrom>());
      return cmm.Memory;
    }
  }

  /// <summary>
  ///   转换到Memory
  /// </summary>
  /// <param name="data"></param>
  /// <typeparam name="TFrom"></typeparam>
  /// <typeparam name="TTo"></typeparam>
  /// <returns></returns>
  public static Memory<TTo> AsMemory<TFrom, TTo>(this Span<TFrom> data)
    where TFrom : struct where TTo : struct
  {
    unsafe
    {
      var ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(data));
      using var cmm = new PointerMemoryManager<TTo>(ptr, data.Length * Unsafe.SizeOf<TFrom>());
      return cmm.Memory;
    }
  }

  private sealed class CastMemoryManager<TFrom, TTo>(Memory<TFrom> from) : MemoryManager<TTo>
    where TFrom : struct
    where TTo : struct
  {
    public override Span<TTo> GetSpan()
    {
      return MemoryMarshal.Cast<TFrom, TTo>(from.Span);
    }

    protected override void Dispose(bool disposing)
    {
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
      throw new NotSupportedException();
    }

    public override void Unpin()
    {
    }
  }

  private sealed unsafe class PointerMemoryManager<T> : MemoryManager<T> where T : struct
  {
    private readonly int _length;
    private readonly void* _pointer;

    internal PointerMemoryManager(void* pointer, int length)
    {
      _pointer = pointer;
      _length = length;
    }

    protected override void Dispose(bool disposing)
    {
    }

    public override Span<T> GetSpan()
    {
      return new Span<T>(_pointer, _length);
    }

    public override MemoryHandle Pin(int elementIndex = 0)
    {
      throw new NotSupportedException();
    }

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
  /// <param name="value">数据</param>
  /// <param name="useBigEndianMode">是否使用大端模式</param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBytes<T>(this T value, bool useBigEndianMode = false) where T : unmanaged
  {
    return ToBytes(value, useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="value"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBytes<T>(this T value, byte mode) where T : unmanaged
  {
    return ToBytes(value, (BigAndSmallEndianEncodingMode)mode);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="value"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static byte[] ToBytes<T>(this T value, BigAndSmallEndianEncodingMode mode) where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    var result = new byte[size];
    WriteTo(value, result.AsSpan(), mode);
    return result;
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="value"></param>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this T value, Span<byte> data, byte mode)
    where T : unmanaged
  {
    WriteTo(value, data, (BigAndSmallEndianEncodingMode)mode);
  }

  /// <summary>
  ///   转换到 byte[]
  /// </summary>
  /// <param name="value"></param>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <typeparam name="T"></typeparam>
  /// <exception cref="ArgumentOutOfRangeException"></exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteTo<T>(this T value, Span<byte> data, BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(data, size);

    if (data.Length > size) data = data[..size];

    Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(data)) = value;

    ApplyEndianness(data, mode);
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
    data.CopyTo(ref value, mode);
    return value;
  }

  /// <summary>
  ///   写入到 T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CopyTo<T>(this ReadOnlySpan<byte> data, ref T value,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(data, size);

    unsafe
    {
      fixed (T* p = &value)
      {
        var span = new Span<byte>(p, size);
        data.CopyTo(span);
        ApplyEndianness(span, mode);
      }
    }
  }

  /// <summary>
  ///   写入到 T
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="data"></param>
  /// <param name="mode"></param>
  /// <param name="value"></param>
  /// <returns></returns>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void CopyTo<T>(this Span<byte> data, ref T value,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var size = Unsafe.SizeOf<T>();
    CheckLength(data, size);

    unsafe
    {
      fixed (T* p = &value)
      {
        var span = new Span<byte>(p, size);
        data.CopyTo(span);
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

    switch (mode)
    {
      case BigAndSmallEndianEncodingMode.DCBA:
        if (!BitConverter.IsLittleEndian) span.Reverse();
        break;
      case BigAndSmallEndianEncodingMode.ABCD:
        if (BitConverter.IsLittleEndian) span.Reverse();
        break;
      case BigAndSmallEndianEncodingMode.BADC:
        // 二字节翻转，前后不翻转
        // 已经判断过，必须为 2 的倍数

        for (var i = 0; i < size; i += 2)
        {
          var sp = span.Slice(i, 2);
          sp.Reverse();
        }

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