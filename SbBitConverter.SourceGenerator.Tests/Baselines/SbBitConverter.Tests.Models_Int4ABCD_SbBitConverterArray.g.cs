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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
partial struct Int4ABCD
{
  public Int4ABCD(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Int4ABCD>());
    this._item0 = data.Slice(0, 4).ToT<int>(mode);
    this._item1 = data.Slice(4, 4).ToT<int>(mode);
    this._item2 = data.Slice(8, 4).ToT<int>(mode);
    this._item3 = data.Slice(12, 4).ToT<int>(mode);
  }

  [FieldOffset(0)]private int _item0;

  [FieldOffset(4)]private int _item1;

  [FieldOffset(8)]private int _item2;

  [FieldOffset(12)]private int _item3;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    var data = new byte[Unsafe.SizeOf<Int4ABCD>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(span, Unsafe.SizeOf<Int4ABCD>());
    this._item0.WriteTo<int>(span.Slice(0, 4), mode);
    this._item1.WriteTo<int>(span.Slice(4, 4), mode);
    this._item2.WriteTo<int>(span.Slice(8, 4), mode);
    this._item3.WriteTo<int>(span.Slice(12, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<Int4ABCD>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
    this._item2.ReadFromBytes(data.Slice(8, 4), mode);
    this._item3.ReadFromBytes(data.Slice(12, 4), mode);
  }

  public int Length => 4;
  public int Count => 4;

  public ref int this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<int> AsSpan()
  {
    return CreateSpan(ref _item0, 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<int> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
