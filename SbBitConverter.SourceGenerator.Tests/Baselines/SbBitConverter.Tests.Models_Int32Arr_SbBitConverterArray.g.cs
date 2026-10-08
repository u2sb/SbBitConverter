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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 128)]
partial struct Int32Arr
{
  public Int32Arr(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int32Arr>());
    this._item0 = data.Slice(0, 4).ToT<int>(mode);
    this._item1 = data.Slice(4, 4).ToT<int>(mode);
    this._item2 = data.Slice(8, 4).ToT<int>(mode);
    this._item3 = data.Slice(12, 4).ToT<int>(mode);
    this._item4 = data.Slice(16, 4).ToT<int>(mode);
    this._item5 = data.Slice(20, 4).ToT<int>(mode);
    this._item6 = data.Slice(24, 4).ToT<int>(mode);
    this._item7 = data.Slice(28, 4).ToT<int>(mode);
    this._item8 = data.Slice(32, 4).ToT<int>(mode);
    this._item9 = data.Slice(36, 4).ToT<int>(mode);
    this._item10 = data.Slice(40, 4).ToT<int>(mode);
    this._item11 = data.Slice(44, 4).ToT<int>(mode);
    this._item12 = data.Slice(48, 4).ToT<int>(mode);
    this._item13 = data.Slice(52, 4).ToT<int>(mode);
    this._item14 = data.Slice(56, 4).ToT<int>(mode);
    this._item15 = data.Slice(60, 4).ToT<int>(mode);
    this._item16 = data.Slice(64, 4).ToT<int>(mode);
    this._item17 = data.Slice(68, 4).ToT<int>(mode);
    this._item18 = data.Slice(72, 4).ToT<int>(mode);
    this._item19 = data.Slice(76, 4).ToT<int>(mode);
    this._item20 = data.Slice(80, 4).ToT<int>(mode);
    this._item21 = data.Slice(84, 4).ToT<int>(mode);
    this._item22 = data.Slice(88, 4).ToT<int>(mode);
    this._item23 = data.Slice(92, 4).ToT<int>(mode);
    this._item24 = data.Slice(96, 4).ToT<int>(mode);
    this._item25 = data.Slice(100, 4).ToT<int>(mode);
    this._item26 = data.Slice(104, 4).ToT<int>(mode);
    this._item27 = data.Slice(108, 4).ToT<int>(mode);
    this._item28 = data.Slice(112, 4).ToT<int>(mode);
    this._item29 = data.Slice(116, 4).ToT<int>(mode);
    this._item30 = data.Slice(120, 4).ToT<int>(mode);
    this._item31 = data.Slice(124, 4).ToT<int>(mode);
  }

  [FieldOffset(0)]private int _item0;

  [FieldOffset(4)]private int _item1;

  [FieldOffset(8)]private int _item2;

  [FieldOffset(12)]private int _item3;

  [FieldOffset(16)]private int _item4;

  [FieldOffset(20)]private int _item5;

  [FieldOffset(24)]private int _item6;

  [FieldOffset(28)]private int _item7;

  [FieldOffset(32)]private int _item8;

  [FieldOffset(36)]private int _item9;

  [FieldOffset(40)]private int _item10;

  [FieldOffset(44)]private int _item11;

  [FieldOffset(48)]private int _item12;

  [FieldOffset(52)]private int _item13;

  [FieldOffset(56)]private int _item14;

  [FieldOffset(60)]private int _item15;

  [FieldOffset(64)]private int _item16;

  [FieldOffset(68)]private int _item17;

  [FieldOffset(72)]private int _item18;

  [FieldOffset(76)]private int _item19;

  [FieldOffset(80)]private int _item20;

  [FieldOffset(84)]private int _item21;

  [FieldOffset(88)]private int _item22;

  [FieldOffset(92)]private int _item23;

  [FieldOffset(96)]private int _item24;

  [FieldOffset(100)]private int _item25;

  [FieldOffset(104)]private int _item26;

  [FieldOffset(108)]private int _item27;

  [FieldOffset(112)]private int _item28;

  [FieldOffset(116)]private int _item29;

  [FieldOffset(120)]private int _item30;

  [FieldOffset(124)]private int _item31;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Int32Arr>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Int32Arr>());
    this._item0.WriteTo<int>(span.Slice(0, 4), mode);
    this._item1.WriteTo<int>(span.Slice(4, 4), mode);
    this._item2.WriteTo<int>(span.Slice(8, 4), mode);
    this._item3.WriteTo<int>(span.Slice(12, 4), mode);
    this._item4.WriteTo<int>(span.Slice(16, 4), mode);
    this._item5.WriteTo<int>(span.Slice(20, 4), mode);
    this._item6.WriteTo<int>(span.Slice(24, 4), mode);
    this._item7.WriteTo<int>(span.Slice(28, 4), mode);
    this._item8.WriteTo<int>(span.Slice(32, 4), mode);
    this._item9.WriteTo<int>(span.Slice(36, 4), mode);
    this._item10.WriteTo<int>(span.Slice(40, 4), mode);
    this._item11.WriteTo<int>(span.Slice(44, 4), mode);
    this._item12.WriteTo<int>(span.Slice(48, 4), mode);
    this._item13.WriteTo<int>(span.Slice(52, 4), mode);
    this._item14.WriteTo<int>(span.Slice(56, 4), mode);
    this._item15.WriteTo<int>(span.Slice(60, 4), mode);
    this._item16.WriteTo<int>(span.Slice(64, 4), mode);
    this._item17.WriteTo<int>(span.Slice(68, 4), mode);
    this._item18.WriteTo<int>(span.Slice(72, 4), mode);
    this._item19.WriteTo<int>(span.Slice(76, 4), mode);
    this._item20.WriteTo<int>(span.Slice(80, 4), mode);
    this._item21.WriteTo<int>(span.Slice(84, 4), mode);
    this._item22.WriteTo<int>(span.Slice(88, 4), mode);
    this._item23.WriteTo<int>(span.Slice(92, 4), mode);
    this._item24.WriteTo<int>(span.Slice(96, 4), mode);
    this._item25.WriteTo<int>(span.Slice(100, 4), mode);
    this._item26.WriteTo<int>(span.Slice(104, 4), mode);
    this._item27.WriteTo<int>(span.Slice(108, 4), mode);
    this._item28.WriteTo<int>(span.Slice(112, 4), mode);
    this._item29.WriteTo<int>(span.Slice(116, 4), mode);
    this._item30.WriteTo<int>(span.Slice(120, 4), mode);
    this._item31.WriteTo<int>(span.Slice(124, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int32Arr>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
    this._item2.ReadFromBytes(data.Slice(8, 4), mode);
    this._item3.ReadFromBytes(data.Slice(12, 4), mode);
    this._item4.ReadFromBytes(data.Slice(16, 4), mode);
    this._item5.ReadFromBytes(data.Slice(20, 4), mode);
    this._item6.ReadFromBytes(data.Slice(24, 4), mode);
    this._item7.ReadFromBytes(data.Slice(28, 4), mode);
    this._item8.ReadFromBytes(data.Slice(32, 4), mode);
    this._item9.ReadFromBytes(data.Slice(36, 4), mode);
    this._item10.ReadFromBytes(data.Slice(40, 4), mode);
    this._item11.ReadFromBytes(data.Slice(44, 4), mode);
    this._item12.ReadFromBytes(data.Slice(48, 4), mode);
    this._item13.ReadFromBytes(data.Slice(52, 4), mode);
    this._item14.ReadFromBytes(data.Slice(56, 4), mode);
    this._item15.ReadFromBytes(data.Slice(60, 4), mode);
    this._item16.ReadFromBytes(data.Slice(64, 4), mode);
    this._item17.ReadFromBytes(data.Slice(68, 4), mode);
    this._item18.ReadFromBytes(data.Slice(72, 4), mode);
    this._item19.ReadFromBytes(data.Slice(76, 4), mode);
    this._item20.ReadFromBytes(data.Slice(80, 4), mode);
    this._item21.ReadFromBytes(data.Slice(84, 4), mode);
    this._item22.ReadFromBytes(data.Slice(88, 4), mode);
    this._item23.ReadFromBytes(data.Slice(92, 4), mode);
    this._item24.ReadFromBytes(data.Slice(96, 4), mode);
    this._item25.ReadFromBytes(data.Slice(100, 4), mode);
    this._item26.ReadFromBytes(data.Slice(104, 4), mode);
    this._item27.ReadFromBytes(data.Slice(108, 4), mode);
    this._item28.ReadFromBytes(data.Slice(112, 4), mode);
    this._item29.ReadFromBytes(data.Slice(116, 4), mode);
    this._item30.ReadFromBytes(data.Slice(120, 4), mode);
    this._item31.ReadFromBytes(data.Slice(124, 4), mode);
  }

  public int Length => 32;
  public int Count => 32;

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
    return CreateSpan(ref _item0, 32);
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
