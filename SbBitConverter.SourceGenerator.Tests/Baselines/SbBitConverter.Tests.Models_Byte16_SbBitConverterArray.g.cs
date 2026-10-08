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
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
partial struct Byte16
{
  public Byte16(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Byte16>());
    this._item0 = data[0];
    this._item1 = data[1];
    this._item2 = data[2];
    this._item3 = data[3];
    this._item4 = data[4];
    this._item5 = data[5];
    this._item6 = data[6];
    this._item7 = data[7];
    this._item8 = data[8];
    this._item9 = data[9];
    this._item10 = data[10];
    this._item11 = data[11];
    this._item12 = data[12];
    this._item13 = data[13];
    this._item14 = data[14];
    this._item15 = data[15];
  }

  [FieldOffset(0)]private byte _item0;

  [FieldOffset(1)]private byte _item1;

  [FieldOffset(2)]private byte _item2;

  [FieldOffset(3)]private byte _item3;

  [FieldOffset(4)]private byte _item4;

  [FieldOffset(5)]private byte _item5;

  [FieldOffset(6)]private byte _item6;

  [FieldOffset(7)]private byte _item7;

  [FieldOffset(8)]private byte _item8;

  [FieldOffset(9)]private byte _item9;

  [FieldOffset(10)]private byte _item10;

  [FieldOffset(11)]private byte _item11;

  [FieldOffset(12)]private byte _item12;

  [FieldOffset(13)]private byte _item13;

  [FieldOffset(14)]private byte _item14;

  [FieldOffset(15)]private byte _item15;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Byte16>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Byte16>());
    AsSpan().CopyTo(span);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Byte16>());
    this._item0 = data[0];
    this._item1 = data[1];
    this._item2 = data[2];
    this._item3 = data[3];
    this._item4 = data[4];
    this._item5 = data[5];
    this._item6 = data[6];
    this._item7 = data[7];
    this._item8 = data[8];
    this._item9 = data[9];
    this._item10 = data[10];
    this._item11 = data[11];
    this._item12 = data[12];
    this._item13 = data[13];
    this._item14 = data[14];
    this._item15 = data[15];
  }

  public int Length => 16;
  public int Count => 16;

  public ref byte this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<byte> AsSpan()
  {
    return CreateSpan(ref _item0, 16);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<byte> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
