using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SbBitConverter.Attributes;

namespace SbBitConverter.Utils;

/// <summary>
///   转换类
/// </summary>
public static class BitConverter
{
  #region 大小端转换

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static void ApplyEndianness(in Span<byte> span, BigAndSmallEndianEncodingMode mode)
  {
    // 如果是单字节，也就是 byte 类型，直接返回
    var size = span.Length;
    if (size == 1) return;

    if (span.Length % 2 != 0)
      throw new ArgumentException("Data length must be even.");

    // 小端模式 初始为 DCBA
    if (System.BitConverter.IsLittleEndian)
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

  /// <summary>
  ///   byte 拓展
  /// </summary>
  extension(byte source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte WithEndianness(bool useBigEndianMode = false)
    {
      return source;
    }
  }

  /// <summary>
  ///   byte 拓展
  /// </summary>
  extension(ref byte source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
    }
  }

  /// <summary>
  ///   sbyte 拓展
  /// </summary>
  extension(sbyte source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public sbyte WithEndianness(bool useBigEndianMode = false)
    {
      return source;
    }
  }

  /// <summary>
  ///   sbyte 拓展
  /// </summary>
  extension(ref sbyte source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
    }
  }

  /// <summary>
  ///   short 拓展
  /// </summary>
  extension(short source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   short 拓展
  /// </summary>
  extension(ref short source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ushort 拓展
  /// </summary>
  extension(ushort source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ushort 拓展
  /// </summary>
  extension(ref ushort source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   int 拓展
  /// </summary>
  extension(int source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   int 拓展
  /// </summary>
  extension(ref int source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }


  /// <summary>
  ///   uint 拓展
  /// </summary>
  extension(uint source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   uint 拓展
  /// </summary>
  extension(ref uint source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   long 拓展
  /// </summary>
  extension(long source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   long 拓展
  /// </summary>
  extension(ref long source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ulong 拓展
  /// </summary>
  extension(ulong source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ulong 拓展
  /// </summary>
  extension(ref ulong source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   float 拓展
  /// </summary>
  extension(float source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float WithEndianness(bool useBigEndianMode = false)
    {
      var span = source.AsReadOnlyByteSpan();
      return span.ToT<float>(useBigEndianMode);
    }
  }

  /// <summary>
  ///   float 拓展
  /// </summary>
  extension(ref float source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode)
      {
        var span = source.AsByteSpan();
        ApplyEndianness(in span,
          useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
      }
    }
  }

  /// <summary>
  ///   double 拓展
  /// </summary>
  extension(double source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double WithEndianness(bool useBigEndianMode = false)
    {
      var span = source.AsReadOnlyByteSpan();
      return span.ToT<double>(useBigEndianMode);
    }
  }

  /// <summary>
  ///   double 拓展
  /// </summary>
  extension(ref double source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode)
      {
        var span = source.AsByteSpan();
        ApplyEndianness(in span,
          useBigEndianMode ? BigAndSmallEndianEncodingMode.ABCD : BigAndSmallEndianEncodingMode.DCBA);
      }
    }
  }


  /// <summary>
  ///   Memory 转换
  /// </summary>
  /// <typeparam name="TFrom"></typeparam>
  extension<TFrom>(Memory<TFrom> source) where TFrom : unmanaged
  {
    /// <summary>
    ///   Memory 转换
    /// </summary>
    /// <typeparam name="TTo"></typeparam>
    /// <returns></returns>
    public Memory<TTo> Cast<TTo>()
      where TTo : unmanaged
    {
      if (typeof(TFrom) == typeof(TTo))
        return (Memory<TTo>)(object)source;

      return new CastMemoryManager<TFrom, TTo>(source).Memory;
    }
  }

  /// <summary>
  ///   通用 转换
  /// </summary>
  /// <typeparam name="T"></typeparam>
  extension<T>(ref T source) where T : unmanaged
  {
    /// <summary>
    ///   解释为为 ByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<byte> AsByteSpan()
    {
      return MemoryMarshal.CreateSpan(ref Unsafe.As<T, byte>(ref source), Unsafe.SizeOf<T>());
    }

    /// <summary>
    ///   解释为为 ByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref source), Unsafe.SizeOf<T>());
    }
  }

  /// <summary>
  ///   通用 转换
  /// </summary>
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
    public void WriteTo(in Span<byte> destination, BigAndSmallEndianEncodingMode mode)
    {
      var size = Unsafe.SizeOf<T>();
#if NET8_0_OR_GREATER
      MemoryMarshal.Write(destination, in source);
#else
      MemoryMarshal.Write(destination, ref source);
#endif
      ApplyEndianness(destination[..size], mode);
    }
  }

  /// <summary>
  ///   ReadOnlySpan 拓展
  /// </summary>
  extension(in ReadOnlySpan<byte> source)
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
      ApplyEndianness(in span, mode);
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
  extension<T>(in ReadOnlySpan<T> source) where T : unmanaged
  {
    /// <summary>
    ///   解释为Span
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan()
    {
      return MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(source), source.Length);
    }
  }

  /// <summary>
  ///   Span 拓展
  /// </summary>
  extension(in Span<byte> source)
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
      ApplyEndianness(in span, mode);
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

  #region Memory转换

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

#if NET8_0_OR_GREATER
  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(Int128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Int128 WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(ref Int128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(UInt128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public UInt128 WithEndianness(bool useBigEndianMode = false)
    {
      return System.BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(ref UInt128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (System.BitConverter.IsLittleEndian == useBigEndianMode) source = BinaryPrimitives.ReverseEndianness(source);
    }
  }

#endif
}