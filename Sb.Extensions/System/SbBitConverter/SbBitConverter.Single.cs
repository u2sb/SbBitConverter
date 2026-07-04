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
  ///   float 拓展
  /// </summary>
  /// <param name="source"></param>
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
  /// <param name="source"></param>
  extension(scoped ref float source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (BitConverter.IsLittleEndian == useBigEndianMode)
      {
        var span = source.AsByteSpan();
        span.ApplyEndianness(useBigEndianMode
          ? BigAndSmallEndianEncodingMode.ABCD
          : BigAndSmallEndianEncodingMode.DCBA);
      }
    }
  }

  /// <summary>
  ///   float 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in float source)
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
  ///   double 拓展
  /// </summary>
  /// <param name="source"></param>
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
  /// <param name="source"></param>
  extension(scoped ref double source)
  {
    /// <summary>
    ///   应用大小端
    /// </summary>
    /// <param name="useBigEndianMode"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ApplyEndianness(bool useBigEndianMode = false)
    {
      if (BitConverter.IsLittleEndian == useBigEndianMode)
      {
        var span = source.AsByteSpan();
        span.ApplyEndianness(useBigEndianMode
          ? BigAndSmallEndianEncodingMode.ABCD
          : BigAndSmallEndianEncodingMode.DCBA);
      }
    }
  }

  /// <summary>
  ///   double 拓展
  /// </summary>
  /// <param name="source"></param>
  extension(scoped in double source)
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
