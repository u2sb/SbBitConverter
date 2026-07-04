using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CommunityToolkit.HighPerformance;

#pragma warning disable IDE0130
namespace Sb.Extensions.System;

partial class SbBitConverter
{
  /// <summary>
  ///   byte 拓展
  /// </summary>
  /// <param name="source"></param>
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
  /// <param name="source"></param>
  extension(scoped ref byte source)
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
  ///   byte 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in byte source)
  {
    /// <summary>
    ///   解释为 ReadOnlyByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return SpanExtension.CreateReadOnlySpan(in source);
    }
  }

  /// <summary>
  ///   sbyte 拓展
  /// </summary>
  /// <param name="source"></param>
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
  /// <param name="source"></param>
  extension(scoped ref sbyte source)
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
  /// <param name="source"></param>
  extension(scoped in sbyte source)
  {
    /// <summary>
    ///   解释为 ReadOnlyByteSpan
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<byte> AsReadOnlyByteSpan()
    {
      return MemoryMarshal.AsBytes(SpanExtension.CreateReadOnlySpan(in source));
    }
  }
}
