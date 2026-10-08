using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#pragma warning disable IDE0130
namespace Sb.Extensions.System;

public static partial class SbBitConverter
{
  /// <summary>
  ///   定长容器的<strong>整体</strong>读写路径（批量转端）。
  ///   <para>
  ///     与既有逐元素 <c>ToT&lt;T&gt;(mode)</c> / <c>WriteTo&lt;T&gt;(…)</c> 的差别：
  ///     先把整个负载一次 <c>CopyTo</c>（向量化 memcpy），再<strong>按元素尺寸</strong>就地应用大小端，
  ///     从而把「每元素一次的尺寸校验 + mode 分派」降为「每次调用一次」。
  ///   </para>
  ///   <para>
  ///     ⚠️ 奇数元素尺寸在任何模式下都会抛 <see cref="ArgumentException" />，<strong>包括本应无操作的 <c>DCBA</c></strong>
  ///     —— 这是既有行为（校验位于 mode 分派之前），必须原样保留。
  ///   </para>
  /// </summary>
  /// <summary>把字节数据整体读入定长容器，并按 <paramref name="mode" /> 逐元素转换。</summary>
  /// <typeparam name="T">元素类型。</typeparam>
  /// <param name="source">源字节。</param>
  /// <param name="destination">容器内存（长度须与负载元素数一致）。</param>
  /// <param name="elementSize">元素字节数（来自特性声明，可能不同于 <c>Unsafe.SizeOf&lt;T&gt;()</c>）。</param>
  /// <param name="mode">编码模式。</param>
  /// <exception cref="InvalidArrayLengthException">源字节长度不足。</exception>
  /// <exception cref="ArgumentException">元素字节数为奇数。</exception>
  /// <exception cref="ArgumentOutOfRangeException">编码模式非法。</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void ReadBulk<T>(
    ReadOnlySpan<byte> source,
    Span<T> destination,
    int elementSize,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var bytes = MemoryMarshal.AsBytes(destination);
    CheckLength(source, bytes.Length);
    source[..bytes.Length].CopyTo(bytes);
    ApplyEndiannessPerElement(bytes, elementSize, mode);
  }

  /// <summary>把定长容器的整体内容按 <paramref name="mode" /> 逐元素转换后写出。</summary>
  /// <typeparam name="T">元素类型。</typeparam>
  /// <param name="source">容器内存。</param>
  /// <param name="destination">目标字节缓冲。</param>
  /// <param name="elementSize">元素字节数（来自特性声明）。</param>
  /// <param name="mode">编码模式。</param>
  /// <exception cref="InvalidArrayLengthException">目标字节缓冲长度不足。</exception>
  /// <exception cref="ArgumentException">元素字节数为奇数。</exception>
  /// <exception cref="ArgumentOutOfRangeException">编码模式非法。</exception>
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static void WriteBulk<T>(
    ReadOnlySpan<T> source,
    Span<byte> destination,
    int elementSize,
    BigAndSmallEndianEncodingMode mode)
    where T : unmanaged
  {
    var bytes = MemoryMarshal.AsBytes(source);
    CheckLength(destination, bytes.Length);
    bytes.CopyTo(destination);
    ApplyEndiannessPerElement(destination[..bytes.Length], elementSize, mode);
  }

  /// <summary>
  ///   按元素尺寸就地应用大小端。语义与逐元素调用既有 <c>ApplyEndianness(mode)</c> 一致，
  ///   但把 mode 分派提升到循环之外，并对可统一的模式改用整缓冲单次扫描。
  /// </summary>
  /// <param name="buffer">待转换的字节缓冲（长度须为 <paramref name="elementSize" /> 的整数倍）。</param>
  /// <param name="elementSize">元素字节数。</param>
  /// <param name="mode">编码模式。</param>
  /// <exception cref="ArgumentException">元素字节数为奇数。</exception>
  /// <exception cref="ArgumentOutOfRangeException">编码模式非法。</exception>
  public static void ApplyEndiannessPerElement(
    Span<byte> buffer,
    int elementSize,
    BigAndSmallEndianEncodingMode mode)
  {
    // 单字节元素，4 种模式均恒等（与既有 ApplyEndianness 的 size == 1 快路径一致）。
    // 空缓冲同样直接返回（零次元素迭代即零次校验）。
    if (buffer.Length == 0 || elementSize == 1)
    {
      return;
    }

    // 既有行为：奇数元素尺寸抛异常，且该校验位于 mode 分派之前 —— DCBA 也抛。
    if (elementSize % 2 != 0)
    {
      throw new ArgumentException("Data length must be even.");
    }

    if (BitConverter.IsLittleEndian)
    {
      // 小端主机：本机字节序即 DCBA
      switch (mode)
      {
        case BigAndSmallEndianEncodingMode.DCBA: // 本机布局即目标 → 纯 memcpy，已在上游完成
          return;
        case BigAndSmallEndianEncodingMode.ABCD:
          ReverseEachElement(buffer, elementSize);
          return;
        case BigAndSmallEndianEncodingMode.BADC:
          ReverseWordOrderEachElement(buffer, elementSize);
          return;
        case BigAndSmallEndianEncodingMode.CDAB:
          SwapBytesWithinEachWord(buffer);
          return;
        default:
          throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
      }
    }

    // 大端主机：本机字节序即 ABCD
    switch (mode)
    {
      case BigAndSmallEndianEncodingMode.ABCD:
        return;
      case BigAndSmallEndianEncodingMode.DCBA:
        ReverseEachElement(buffer, elementSize);
        return;
      case BigAndSmallEndianEncodingMode.BADC:
        SwapBytesWithinEachWord(buffer);
        return;
      case BigAndSmallEndianEncodingMode.CDAB:
        ReverseWordOrderEachElement(buffer, elementSize);
        return;
      default:
        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
    }
  }

  /// <summary>逐元素整段翻转（mode 分派已在调用方提出循环）。</summary>
  private static void ReverseEachElement(Span<byte> buffer, int elementSize)
  {
    for (var offset = 0; offset < buffer.Length; offset += elementSize)
    {
      buffer.Slice(offset, elementSize).Reverse();
    }
  }

  /// <summary>逐元素翻转「二字节单元」的先后顺序（单元内部不变）。</summary>
  private static void ReverseWordOrderEachElement(Span<byte> buffer, int elementSize)
  {
    for (var offset = 0; offset < buffer.Length; offset += elementSize)
    {
      MemoryMarshal.Cast<byte, ushort>(buffer.Slice(offset, elementSize)).Reverse();
    }
  }

  /// <summary>
  ///   交换每个二字节单元内部的字节。该操作与元素边界无关，
  ///   故可对<strong>整个缓冲一次扫描</strong>，无需元素循环。
  /// </summary>
  private static void SwapBytesWithinEachWord(Span<byte> buffer)
  {
    foreach (ref var word in MemoryMarshal.Cast<byte, ushort>(buffer))
    {
      word = BinaryPrimitives.ReverseEndianness(word);
    }
  }
}
