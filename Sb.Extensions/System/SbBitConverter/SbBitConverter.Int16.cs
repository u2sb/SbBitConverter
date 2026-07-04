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
  ///   short 拓展
  /// </summary>
  /// <param name="source"></param>
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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   short 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped ref short source)
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
  ///   short 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in short source)
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
  ///   ushort 拓展
  /// </summary>
  /// <param name="source"></param>
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
      return BitConverter.IsLittleEndian != useBigEndianMode
        ? source
        : BinaryPrimitives.ReverseEndianness(source);
    }
  }

  /// <summary>
  ///   ushort 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped ref ushort source)
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
  ///   ushort 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in ushort source)
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
