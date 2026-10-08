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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 12)]
partial struct Float3
{
  public Float3(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Float3>());
    this._item0 = data.Slice(0, 4).ToT<float>(mode);
    this._item1 = data.Slice(4, 4).ToT<float>(mode);
    this._item2 = data.Slice(8, 4).ToT<float>(mode);
  }

  [FieldOffset(0)]private float _item0;

  [FieldOffset(4)]private float _item1;

  [FieldOffset(8)]private float _item2;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    var data = new byte[Unsafe.SizeOf<Float3>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(span, Unsafe.SizeOf<Float3>());
    this._item0.WriteTo<float>(span.Slice(0, 4), mode);
    this._item1.WriteTo<float>(span.Slice(4, 4), mode);
    this._item2.WriteTo<float>(span.Slice(8, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Float3>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
    this._item2.ReadFromBytes(data.Slice(8, 4), mode);
  }

  public int Length => 3;
  public int Count => 3;

  public ref float this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<float> AsSpan()
  {
    return CreateSpan(ref _item0, 3);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<float> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
