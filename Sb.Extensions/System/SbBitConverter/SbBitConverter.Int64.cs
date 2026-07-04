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
  ///   long 拓展
  /// </summary>
  /// <param name="source"></param>
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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   long 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped ref long source)
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
  ///   long 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in long source)
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

  /// <summary>
  ///   ulong 拓展
  /// </summary>
  /// <param name="source"></param>
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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ulong 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped ref ulong source)
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
  ///   ulong 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in ulong source)
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
