#if NET8_0_OR_GREATER

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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(scoped ref Int128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      source = source.WithEndianness(useBigEndianMode);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in Int128 source)
  {
    /// <summary>
    ///   解释为 ReadOnlyByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return MemoryMarshal.AsBytes(SpanExtension.CreateReadOnlySpan(in source, 1));
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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   Int128 拓展
  /// </summary>
  extension(scoped ref UInt128 source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      source = source.WithEndianness(useBigEndianMode);
    }
  }

  /// <summary>
  ///   UInt128 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in UInt128 source)
  {
    /// <summary>
    ///   解释为 ReadOnlyByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return MemoryMarshal.AsBytes(SpanExtension.CreateReadOnlySpan(in source, 1));
    }
  }
}

#endif
