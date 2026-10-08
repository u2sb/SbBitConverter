// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
[StructLayout(LayoutKind.Explicit, Pack = 8, Size = 16)]
partial struct Double2
{
  public Double2(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Double2>());
    this._item0 = data.Slice(0, 8).ToT<double>(mode);
    this._item1 = data.Slice(8, 8).ToT<double>(mode);
  }

  [FieldOffset(0)]private double _item0;

  [FieldOffset(8)]private double _item1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    var data = new byte[Unsafe.SizeOf<Double2>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(span, Unsafe.SizeOf<Double2>());
    this._item0.WriteTo<double>(span.Slice(0, 8), mode);
    this._item1.WriteTo<double>(span.Slice(8, 8), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Double2>());
    this._item0.ReadFromBytes(data.Slice(0, 8), mode);
    this._item1.ReadFromBytes(data.Slice(8, 8), mode);
  }

  public int Length => 2;
  public int Count => 2;

  public ref double this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<double> AsSpan()
  {
    return CreateSpan(ref _item0, 2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<double> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
