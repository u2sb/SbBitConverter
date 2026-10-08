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
[StructLayout(LayoutKind.Explicit, Pack = 2, Size = 12)]
partial struct Short6
{
  public Short6(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)2)
  {
    CheckLength(data, Unsafe.SizeOf<Short6>());
    this._item0 = data.Slice(0, 2).ToT<short>(mode);
    this._item1 = data.Slice(2, 2).ToT<short>(mode);
    this._item2 = data.Slice(4, 2).ToT<short>(mode);
    this._item3 = data.Slice(6, 2).ToT<short>(mode);
    this._item4 = data.Slice(8, 2).ToT<short>(mode);
    this._item5 = data.Slice(10, 2).ToT<short>(mode);
  }

  [FieldOffset(0)]private short _item0;

  [FieldOffset(2)]private short _item1;

  [FieldOffset(4)]private short _item2;

  [FieldOffset(6)]private short _item3;

  [FieldOffset(8)]private short _item4;

  [FieldOffset(10)]private short _item5;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)2)
  {
    var data = new byte[Unsafe.SizeOf<Short6>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)2)
  {
    CheckLength(span, Unsafe.SizeOf<Short6>());
    this._item0.WriteTo<short>(span.Slice(0, 2), mode);
    this._item1.WriteTo<short>(span.Slice(2, 2), mode);
    this._item2.WriteTo<short>(span.Slice(4, 2), mode);
    this._item3.WriteTo<short>(span.Slice(6, 2), mode);
    this._item4.WriteTo<short>(span.Slice(8, 2), mode);
    this._item5.WriteTo<short>(span.Slice(10, 2), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)2)
  {
    CheckLength(data, Unsafe.SizeOf<Short6>());
    this._item0.ReadFromBytes(data.Slice(0, 2), mode);
    this._item1.ReadFromBytes(data.Slice(2, 2), mode);
    this._item2.ReadFromBytes(data.Slice(4, 2), mode);
    this._item3.ReadFromBytes(data.Slice(6, 2), mode);
    this._item4.ReadFromBytes(data.Slice(8, 2), mode);
    this._item5.ReadFromBytes(data.Slice(10, 2), mode);
  }

  public int Length => 6;
  public int Count => 6;

  public ref short this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<short> AsSpan()
  {
    return CreateSpan(ref _item0, 6);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<short> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
