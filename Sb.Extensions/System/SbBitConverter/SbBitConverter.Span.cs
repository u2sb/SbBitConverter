using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

#pragma warning disable IDE0130
namespace Sb.Extensions.System;

partial class SbBitConverter
{
  /// <summary>
  ///   ReadOnlySpan 拓展
  ///   <param name="source"></param>
  /// </summary>
  extension(scoped in ReadOnlySpan<byte> source)
  {
    /// <summary>
    ///   转换为 short 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ToInt16(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt16BigEndian(source)
        : BinaryPrimitives.ReadInt16LittleEndian(source);
    }

    /// <summary>
    ///   转换为 ushort 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ToUInt16(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt16BigEndian(source)
        : BinaryPrimitives.ReadUInt16LittleEndian(source);
    }

    /// <summary>
    ///   转换为 int 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToInt32(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt32BigEndian(source)
        : BinaryPrimitives.ReadInt32LittleEndian(source);
    }

    /// <summary>
    ///   转换为 uint 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ToUInt32(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt32BigEndian(source)
        : BinaryPrimitives.ReadUInt32LittleEndian(source);
    }

    /// <summary>
    ///   转换为long类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToInt64(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt64BigEndian(source)
        : BinaryPrimitives.ReadInt64LittleEndian(source);
    }

    /// <summary>
    ///   转换为 ulong 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ToUInt64(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt64BigEndian(source)
        : BinaryPrimitives.ReadUInt64LittleEndian(source);
    }

    /// <summary>
    ///   转换为 float 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ToSingle(bool useBigEndianMode = false)
    {
#if NET6_0_OR_GREATER
      return useBigEndianMode
        ? BinaryPrimitives.ReadSingleBigEndian(source)
        : BinaryPrimitives.ReadSingleLittleEndian(source);
#else
      return ToT<float>(source, useBigEndianMode);
#endif
    }

    /// <summary>
    ///   转换为 double 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ToDouble(bool useBigEndianMode = false)
    {
#if NET6_0_OR_GREATER
      return useBigEndianMode
        ? BinaryPrimitives.ReadDoubleBigEndian(source)
        : BinaryPrimitives.ReadDoubleLittleEndian(source);
#else
      return ToT<double>(source, useBigEndianMode);
#endif
    }

    /// <summary>
    ///   转换到T
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ToT<T>(bool useBigEndianMode = false) where T : unmanaged
    {
      return ToT<T>(source, useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
    }

    /// <summary>
    ///   转换到T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ToT<T>(BigAndSmallEndianEncodingMode mode) where T : unmanaged
    {
      T value = default;
      source.WriteTo(ref value, mode);
      return value;
    }

    /// <summary>
    ///   写入到 T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mode"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteTo<T>(ref T destination, BigAndSmallEndianEncodingMode mode) where T : unmanaged
    {
      destination = MemoryMarshal.Read<T>(source);
      var span = AsByteSpan(ref destination);
      span.ApplyEndianness(mode);
    }

#if NET8_0_OR_GREATER
    /// <summary>
    ///   转换为 Int128 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Int128 ToInt128(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt128BigEndian(source)
        : BinaryPrimitives.ReadInt128LittleEndian(source);
    }

    /// <summary>
    ///   转换为 UInt128 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UInt128 ToUInt128(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt128BigEndian(source)
        : BinaryPrimitives.ReadUInt128LittleEndian(source);
    }

#endif
  }

  /// <summary>
  ///   ReadOnlySpan 拓展
  /// </summary>
  /// <param name="source"></param>
  /// <typeparam name="T"></typeparam>
  extension<T>(scoped in ReadOnlySpan<T> source) where T : unmanaged
  {
    /// <summary>
    ///   解释为Span
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
      return SpanExtension.CreateSpan(ref MemoryMarshal.GetReference(source), source.Length);
    }
  }

  /// <summary>
  ///   Span 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in Span<byte> source)
  {
    /// <summary>
    ///   大小端转换
    /// </summary>
    /// <param name="mode"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(BigAndSmallEndianEncodingMode mode)
    {
      // 如果是单字节，也就是 byte 类型，直接返回
      if (source.Length == 1) return;

      if (source.Length % 2 != 0)
        throw new ArgumentException("Data length must be even.");

      // 小端模式 初始为 DCBA
      if (BitConverter.IsLittleEndian)
        switch (mode)
        {
          case BigAndSmallEndianEncodingMode.DCBA:
            break;
          case BigAndSmallEndianEncodingMode.ABCD:
            source.Reverse();
            break;

          // 二字节翻转，前后不翻转 DCBA -> BADC
          case BigAndSmallEndianEncodingMode.BADC:
            // 解释为ushort，然后整体翻转
            var us = MemoryMarshal.Cast<byte, ushort>(source);
            us.Reverse();
            break;

          // 二字节不翻转，前后翻转 DCBA -> CDAB
          case BigAndSmallEndianEncodingMode.CDAB:
            var ushortSpan = MemoryMarshal.Cast<byte, ushort>(source);
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
            source.Reverse();
            break;
          case BigAndSmallEndianEncodingMode.ABCD:
            break;

          // 二字节翻转，前后不翻转 ABCD -> BADC
          case BigAndSmallEndianEncodingMode.BADC:
            var ushortSpan = MemoryMarshal.Cast<byte, ushort>(source);
            foreach (ref var value in ushortSpan) value = BinaryPrimitives.ReverseEndianness(value);
            break;

          // 二字节不翻转，前后翻转 ABCD -> CDAB
          case BigAndSmallEndianEncodingMode.CDAB:
            // 解释为ushort，然后整体翻转
            var us = MemoryMarshal.Cast<byte, ushort>(source);
            us.Reverse();
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }


    /// <summary>
    ///   转换为 short 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ToInt16(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt16BigEndian(source)
        : BinaryPrimitives.ReadInt16LittleEndian(source);
    }

    /// <summary>
    ///   转换为 ushort 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ToUInt16(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt16BigEndian(source)
        : BinaryPrimitives.ReadUInt16LittleEndian(source);
    }

    /// <summary>
    ///   转换为 int 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ToInt32(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt32BigEndian(source)
        : BinaryPrimitives.ReadInt32LittleEndian(source);
    }

    /// <summary>
    ///   转换为 uint 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ToUInt32(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt32BigEndian(source)
        : BinaryPrimitives.ReadUInt32LittleEndian(source);
    }

    /// <summary>
    ///   转换为long类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ToInt64(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt64BigEndian(source)
        : BinaryPrimitives.ReadInt64LittleEndian(source);
    }

    /// <summary>
    ///   转换为 ulong 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ToUInt64(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt64BigEndian(source)
        : BinaryPrimitives.ReadUInt64LittleEndian(source);
    }

    /// <summary>
    ///   转换为 float 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ToSingle(bool useBigEndianMode = false)
    {
#if NET6_0_OR_GREATER
      return useBigEndianMode
        ? BinaryPrimitives.ReadSingleBigEndian(source)
        : BinaryPrimitives.ReadSingleLittleEndian(source);
#else
      return ToT<float>(source, useBigEndianMode);
#endif
    }

    /// <summary>
    ///   转换为 double 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ToDouble(bool useBigEndianMode = false)
    {
#if NET6_0_OR_GREATER
      return useBigEndianMode
        ? BinaryPrimitives.ReadDoubleBigEndian(source)
        : BinaryPrimitives.ReadDoubleLittleEndian(source);
#else
      return ToT<double>(source, useBigEndianMode);
#endif
    }

    /// <summary>
    ///   转换到T
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ToT<T>(bool useBigEndianMode = false) where T : unmanaged
    {
      return ToT<T>(source, useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
    }

    /// <summary>
    ///   转换到T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mode"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T ToT<T>(BigAndSmallEndianEncodingMode mode) where T : unmanaged
    {
      T value = default;
      source.WriteTo(ref value, mode);
      return value;
    }

    /// <summary>
    ///   写入到 T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mode"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteTo<T>(scoped ref T destination, BigAndSmallEndianEncodingMode mode) where T : unmanaged
    {
      destination = MemoryMarshal.Read<T>(source);
      var span = AsByteSpan(ref destination);
      span.ApplyEndianness(mode);
    }

#if NET8_0_OR_GREATER
    /// <summary>
    ///   转换为 Int128 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Int128 ToInt128(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadInt128BigEndian(source)
        : BinaryPrimitives.ReadInt128LittleEndian(source);
    }

    /// <summary>
    ///   转换为 UInt128 类型
    /// </summary>
    /// <param name="useBigEndianMode">是否使用大端模式</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UInt128 ToUInt128(bool useBigEndianMode = false)
    {
      return useBigEndianMode
        ? BinaryPrimitives.ReadUInt128BigEndian(source)
        : BinaryPrimitives.ReadUInt128LittleEndian(source);
    }

#endif
  }
}
