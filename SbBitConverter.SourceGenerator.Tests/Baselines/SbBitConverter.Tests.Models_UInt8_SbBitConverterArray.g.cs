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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 32)]
partial struct UInt8
{
  public UInt8(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<UInt8>());
    this._item0 = data.Slice(0, 4).ToT<uint>(mode);
    this._item1 = data.Slice(4, 4).ToT<uint>(mode);
    this._item2 = data.Slice(8, 4).ToT<uint>(mode);
    this._item3 = data.Slice(12, 4).ToT<uint>(mode);
    this._item4 = data.Slice(16, 4).ToT<uint>(mode);
    this._item5 = data.Slice(20, 4).ToT<uint>(mode);
    this._item6 = data.Slice(24, 4).ToT<uint>(mode);
    this._item7 = data.Slice(28, 4).ToT<uint>(mode);
  }

  [FieldOffset(0)]private uint _item0;

  [FieldOffset(4)]private uint _item1;

  [FieldOffset(8)]private uint _item2;

  [FieldOffset(12)]private uint _item3;

  [FieldOffset(16)]private uint _item4;

  [FieldOffset(20)]private uint _item5;

  [FieldOffset(24)]private uint _item6;

  [FieldOffset(28)]private uint _item7;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<UInt8>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<UInt8>());
    this._item0.WriteTo<uint>(span.Slice(0, 4), mode);
    this._item1.WriteTo<uint>(span.Slice(4, 4), mode);
    this._item2.WriteTo<uint>(span.Slice(8, 4), mode);
    this._item3.WriteTo<uint>(span.Slice(12, 4), mode);
    this._item4.WriteTo<uint>(span.Slice(16, 4), mode);
    this._item5.WriteTo<uint>(span.Slice(20, 4), mode);
    this._item6.WriteTo<uint>(span.Slice(24, 4), mode);
    this._item7.WriteTo<uint>(span.Slice(28, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<UInt8>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
    this._item2.ReadFromBytes(data.Slice(8, 4), mode);
    this._item3.ReadFromBytes(data.Slice(12, 4), mode);
    this._item4.ReadFromBytes(data.Slice(16, 4), mode);
    this._item5.ReadFromBytes(data.Slice(20, 4), mode);
    this._item6.ReadFromBytes(data.Slice(24, 4), mode);
    this._item7.ReadFromBytes(data.Slice(28, 4), mode);
  }

  public int Length => 8;
  public int Count => 8;

  public ref uint this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<uint> AsSpan()
  {
    return CreateSpan(ref _item0, 8);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<uint> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
