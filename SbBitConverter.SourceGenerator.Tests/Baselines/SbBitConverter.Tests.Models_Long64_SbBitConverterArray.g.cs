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
[StructLayout(LayoutKind.Explicit, Pack = 8, Size = 512)]
partial struct Long64
{
  public Long64(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Long64>());
    this._item0 = data.Slice(0, 8).ToT<long>(mode);
    this._item1 = data.Slice(8, 8).ToT<long>(mode);
    this._item2 = data.Slice(16, 8).ToT<long>(mode);
    this._item3 = data.Slice(24, 8).ToT<long>(mode);
    this._item4 = data.Slice(32, 8).ToT<long>(mode);
    this._item5 = data.Slice(40, 8).ToT<long>(mode);
    this._item6 = data.Slice(48, 8).ToT<long>(mode);
    this._item7 = data.Slice(56, 8).ToT<long>(mode);
    this._item8 = data.Slice(64, 8).ToT<long>(mode);
    this._item9 = data.Slice(72, 8).ToT<long>(mode);
    this._item10 = data.Slice(80, 8).ToT<long>(mode);
    this._item11 = data.Slice(88, 8).ToT<long>(mode);
    this._item12 = data.Slice(96, 8).ToT<long>(mode);
    this._item13 = data.Slice(104, 8).ToT<long>(mode);
    this._item14 = data.Slice(112, 8).ToT<long>(mode);
    this._item15 = data.Slice(120, 8).ToT<long>(mode);
    this._item16 = data.Slice(128, 8).ToT<long>(mode);
    this._item17 = data.Slice(136, 8).ToT<long>(mode);
    this._item18 = data.Slice(144, 8).ToT<long>(mode);
    this._item19 = data.Slice(152, 8).ToT<long>(mode);
    this._item20 = data.Slice(160, 8).ToT<long>(mode);
    this._item21 = data.Slice(168, 8).ToT<long>(mode);
    this._item22 = data.Slice(176, 8).ToT<long>(mode);
    this._item23 = data.Slice(184, 8).ToT<long>(mode);
    this._item24 = data.Slice(192, 8).ToT<long>(mode);
    this._item25 = data.Slice(200, 8).ToT<long>(mode);
    this._item26 = data.Slice(208, 8).ToT<long>(mode);
    this._item27 = data.Slice(216, 8).ToT<long>(mode);
    this._item28 = data.Slice(224, 8).ToT<long>(mode);
    this._item29 = data.Slice(232, 8).ToT<long>(mode);
    this._item30 = data.Slice(240, 8).ToT<long>(mode);
    this._item31 = data.Slice(248, 8).ToT<long>(mode);
    this._item32 = data.Slice(256, 8).ToT<long>(mode);
    this._item33 = data.Slice(264, 8).ToT<long>(mode);
    this._item34 = data.Slice(272, 8).ToT<long>(mode);
    this._item35 = data.Slice(280, 8).ToT<long>(mode);
    this._item36 = data.Slice(288, 8).ToT<long>(mode);
    this._item37 = data.Slice(296, 8).ToT<long>(mode);
    this._item38 = data.Slice(304, 8).ToT<long>(mode);
    this._item39 = data.Slice(312, 8).ToT<long>(mode);
    this._item40 = data.Slice(320, 8).ToT<long>(mode);
    this._item41 = data.Slice(328, 8).ToT<long>(mode);
    this._item42 = data.Slice(336, 8).ToT<long>(mode);
    this._item43 = data.Slice(344, 8).ToT<long>(mode);
    this._item44 = data.Slice(352, 8).ToT<long>(mode);
    this._item45 = data.Slice(360, 8).ToT<long>(mode);
    this._item46 = data.Slice(368, 8).ToT<long>(mode);
    this._item47 = data.Slice(376, 8).ToT<long>(mode);
    this._item48 = data.Slice(384, 8).ToT<long>(mode);
    this._item49 = data.Slice(392, 8).ToT<long>(mode);
    this._item50 = data.Slice(400, 8).ToT<long>(mode);
    this._item51 = data.Slice(408, 8).ToT<long>(mode);
    this._item52 = data.Slice(416, 8).ToT<long>(mode);
    this._item53 = data.Slice(424, 8).ToT<long>(mode);
    this._item54 = data.Slice(432, 8).ToT<long>(mode);
    this._item55 = data.Slice(440, 8).ToT<long>(mode);
    this._item56 = data.Slice(448, 8).ToT<long>(mode);
    this._item57 = data.Slice(456, 8).ToT<long>(mode);
    this._item58 = data.Slice(464, 8).ToT<long>(mode);
    this._item59 = data.Slice(472, 8).ToT<long>(mode);
    this._item60 = data.Slice(480, 8).ToT<long>(mode);
    this._item61 = data.Slice(488, 8).ToT<long>(mode);
    this._item62 = data.Slice(496, 8).ToT<long>(mode);
    this._item63 = data.Slice(504, 8).ToT<long>(mode);
  }

  [FieldOffset(0)]private long _item0;

  [FieldOffset(8)]private long _item1;

  [FieldOffset(16)]private long _item2;

  [FieldOffset(24)]private long _item3;

  [FieldOffset(32)]private long _item4;

  [FieldOffset(40)]private long _item5;

  [FieldOffset(48)]private long _item6;

  [FieldOffset(56)]private long _item7;

  [FieldOffset(64)]private long _item8;

  [FieldOffset(72)]private long _item9;

  [FieldOffset(80)]private long _item10;

  [FieldOffset(88)]private long _item11;

  [FieldOffset(96)]private long _item12;

  [FieldOffset(104)]private long _item13;

  [FieldOffset(112)]private long _item14;

  [FieldOffset(120)]private long _item15;

  [FieldOffset(128)]private long _item16;

  [FieldOffset(136)]private long _item17;

  [FieldOffset(144)]private long _item18;

  [FieldOffset(152)]private long _item19;

  [FieldOffset(160)]private long _item20;

  [FieldOffset(168)]private long _item21;

  [FieldOffset(176)]private long _item22;

  [FieldOffset(184)]private long _item23;

  [FieldOffset(192)]private long _item24;

  [FieldOffset(200)]private long _item25;

  [FieldOffset(208)]private long _item26;

  [FieldOffset(216)]private long _item27;

  [FieldOffset(224)]private long _item28;

  [FieldOffset(232)]private long _item29;

  [FieldOffset(240)]private long _item30;

  [FieldOffset(248)]private long _item31;

  [FieldOffset(256)]private long _item32;

  [FieldOffset(264)]private long _item33;

  [FieldOffset(272)]private long _item34;

  [FieldOffset(280)]private long _item35;

  [FieldOffset(288)]private long _item36;

  [FieldOffset(296)]private long _item37;

  [FieldOffset(304)]private long _item38;

  [FieldOffset(312)]private long _item39;

  [FieldOffset(320)]private long _item40;

  [FieldOffset(328)]private long _item41;

  [FieldOffset(336)]private long _item42;

  [FieldOffset(344)]private long _item43;

  [FieldOffset(352)]private long _item44;

  [FieldOffset(360)]private long _item45;

  [FieldOffset(368)]private long _item46;

  [FieldOffset(376)]private long _item47;

  [FieldOffset(384)]private long _item48;

  [FieldOffset(392)]private long _item49;

  [FieldOffset(400)]private long _item50;

  [FieldOffset(408)]private long _item51;

  [FieldOffset(416)]private long _item52;

  [FieldOffset(424)]private long _item53;

  [FieldOffset(432)]private long _item54;

  [FieldOffset(440)]private long _item55;

  [FieldOffset(448)]private long _item56;

  [FieldOffset(456)]private long _item57;

  [FieldOffset(464)]private long _item58;

  [FieldOffset(472)]private long _item59;

  [FieldOffset(480)]private long _item60;

  [FieldOffset(488)]private long _item61;

  [FieldOffset(496)]private long _item62;

  [FieldOffset(504)]private long _item63;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Long64>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Long64>());
    this._item0.WriteTo<long>(span.Slice(0, 8), mode);
    this._item1.WriteTo<long>(span.Slice(8, 8), mode);
    this._item2.WriteTo<long>(span.Slice(16, 8), mode);
    this._item3.WriteTo<long>(span.Slice(24, 8), mode);
    this._item4.WriteTo<long>(span.Slice(32, 8), mode);
    this._item5.WriteTo<long>(span.Slice(40, 8), mode);
    this._item6.WriteTo<long>(span.Slice(48, 8), mode);
    this._item7.WriteTo<long>(span.Slice(56, 8), mode);
    this._item8.WriteTo<long>(span.Slice(64, 8), mode);
    this._item9.WriteTo<long>(span.Slice(72, 8), mode);
    this._item10.WriteTo<long>(span.Slice(80, 8), mode);
    this._item11.WriteTo<long>(span.Slice(88, 8), mode);
    this._item12.WriteTo<long>(span.Slice(96, 8), mode);
    this._item13.WriteTo<long>(span.Slice(104, 8), mode);
    this._item14.WriteTo<long>(span.Slice(112, 8), mode);
    this._item15.WriteTo<long>(span.Slice(120, 8), mode);
    this._item16.WriteTo<long>(span.Slice(128, 8), mode);
    this._item17.WriteTo<long>(span.Slice(136, 8), mode);
    this._item18.WriteTo<long>(span.Slice(144, 8), mode);
    this._item19.WriteTo<long>(span.Slice(152, 8), mode);
    this._item20.WriteTo<long>(span.Slice(160, 8), mode);
    this._item21.WriteTo<long>(span.Slice(168, 8), mode);
    this._item22.WriteTo<long>(span.Slice(176, 8), mode);
    this._item23.WriteTo<long>(span.Slice(184, 8), mode);
    this._item24.WriteTo<long>(span.Slice(192, 8), mode);
    this._item25.WriteTo<long>(span.Slice(200, 8), mode);
    this._item26.WriteTo<long>(span.Slice(208, 8), mode);
    this._item27.WriteTo<long>(span.Slice(216, 8), mode);
    this._item28.WriteTo<long>(span.Slice(224, 8), mode);
    this._item29.WriteTo<long>(span.Slice(232, 8), mode);
    this._item30.WriteTo<long>(span.Slice(240, 8), mode);
    this._item31.WriteTo<long>(span.Slice(248, 8), mode);
    this._item32.WriteTo<long>(span.Slice(256, 8), mode);
    this._item33.WriteTo<long>(span.Slice(264, 8), mode);
    this._item34.WriteTo<long>(span.Slice(272, 8), mode);
    this._item35.WriteTo<long>(span.Slice(280, 8), mode);
    this._item36.WriteTo<long>(span.Slice(288, 8), mode);
    this._item37.WriteTo<long>(span.Slice(296, 8), mode);
    this._item38.WriteTo<long>(span.Slice(304, 8), mode);
    this._item39.WriteTo<long>(span.Slice(312, 8), mode);
    this._item40.WriteTo<long>(span.Slice(320, 8), mode);
    this._item41.WriteTo<long>(span.Slice(328, 8), mode);
    this._item42.WriteTo<long>(span.Slice(336, 8), mode);
    this._item43.WriteTo<long>(span.Slice(344, 8), mode);
    this._item44.WriteTo<long>(span.Slice(352, 8), mode);
    this._item45.WriteTo<long>(span.Slice(360, 8), mode);
    this._item46.WriteTo<long>(span.Slice(368, 8), mode);
    this._item47.WriteTo<long>(span.Slice(376, 8), mode);
    this._item48.WriteTo<long>(span.Slice(384, 8), mode);
    this._item49.WriteTo<long>(span.Slice(392, 8), mode);
    this._item50.WriteTo<long>(span.Slice(400, 8), mode);
    this._item51.WriteTo<long>(span.Slice(408, 8), mode);
    this._item52.WriteTo<long>(span.Slice(416, 8), mode);
    this._item53.WriteTo<long>(span.Slice(424, 8), mode);
    this._item54.WriteTo<long>(span.Slice(432, 8), mode);
    this._item55.WriteTo<long>(span.Slice(440, 8), mode);
    this._item56.WriteTo<long>(span.Slice(448, 8), mode);
    this._item57.WriteTo<long>(span.Slice(456, 8), mode);
    this._item58.WriteTo<long>(span.Slice(464, 8), mode);
    this._item59.WriteTo<long>(span.Slice(472, 8), mode);
    this._item60.WriteTo<long>(span.Slice(480, 8), mode);
    this._item61.WriteTo<long>(span.Slice(488, 8), mode);
    this._item62.WriteTo<long>(span.Slice(496, 8), mode);
    this._item63.WriteTo<long>(span.Slice(504, 8), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Long64>());
    this._item0.ReadFromBytes(data.Slice(0, 8), mode);
    this._item1.ReadFromBytes(data.Slice(8, 8), mode);
    this._item2.ReadFromBytes(data.Slice(16, 8), mode);
    this._item3.ReadFromBytes(data.Slice(24, 8), mode);
    this._item4.ReadFromBytes(data.Slice(32, 8), mode);
    this._item5.ReadFromBytes(data.Slice(40, 8), mode);
    this._item6.ReadFromBytes(data.Slice(48, 8), mode);
    this._item7.ReadFromBytes(data.Slice(56, 8), mode);
    this._item8.ReadFromBytes(data.Slice(64, 8), mode);
    this._item9.ReadFromBytes(data.Slice(72, 8), mode);
    this._item10.ReadFromBytes(data.Slice(80, 8), mode);
    this._item11.ReadFromBytes(data.Slice(88, 8), mode);
    this._item12.ReadFromBytes(data.Slice(96, 8), mode);
    this._item13.ReadFromBytes(data.Slice(104, 8), mode);
    this._item14.ReadFromBytes(data.Slice(112, 8), mode);
    this._item15.ReadFromBytes(data.Slice(120, 8), mode);
    this._item16.ReadFromBytes(data.Slice(128, 8), mode);
    this._item17.ReadFromBytes(data.Slice(136, 8), mode);
    this._item18.ReadFromBytes(data.Slice(144, 8), mode);
    this._item19.ReadFromBytes(data.Slice(152, 8), mode);
    this._item20.ReadFromBytes(data.Slice(160, 8), mode);
    this._item21.ReadFromBytes(data.Slice(168, 8), mode);
    this._item22.ReadFromBytes(data.Slice(176, 8), mode);
    this._item23.ReadFromBytes(data.Slice(184, 8), mode);
    this._item24.ReadFromBytes(data.Slice(192, 8), mode);
    this._item25.ReadFromBytes(data.Slice(200, 8), mode);
    this._item26.ReadFromBytes(data.Slice(208, 8), mode);
    this._item27.ReadFromBytes(data.Slice(216, 8), mode);
    this._item28.ReadFromBytes(data.Slice(224, 8), mode);
    this._item29.ReadFromBytes(data.Slice(232, 8), mode);
    this._item30.ReadFromBytes(data.Slice(240, 8), mode);
    this._item31.ReadFromBytes(data.Slice(248, 8), mode);
    this._item32.ReadFromBytes(data.Slice(256, 8), mode);
    this._item33.ReadFromBytes(data.Slice(264, 8), mode);
    this._item34.ReadFromBytes(data.Slice(272, 8), mode);
    this._item35.ReadFromBytes(data.Slice(280, 8), mode);
    this._item36.ReadFromBytes(data.Slice(288, 8), mode);
    this._item37.ReadFromBytes(data.Slice(296, 8), mode);
    this._item38.ReadFromBytes(data.Slice(304, 8), mode);
    this._item39.ReadFromBytes(data.Slice(312, 8), mode);
    this._item40.ReadFromBytes(data.Slice(320, 8), mode);
    this._item41.ReadFromBytes(data.Slice(328, 8), mode);
    this._item42.ReadFromBytes(data.Slice(336, 8), mode);
    this._item43.ReadFromBytes(data.Slice(344, 8), mode);
    this._item44.ReadFromBytes(data.Slice(352, 8), mode);
    this._item45.ReadFromBytes(data.Slice(360, 8), mode);
    this._item46.ReadFromBytes(data.Slice(368, 8), mode);
    this._item47.ReadFromBytes(data.Slice(376, 8), mode);
    this._item48.ReadFromBytes(data.Slice(384, 8), mode);
    this._item49.ReadFromBytes(data.Slice(392, 8), mode);
    this._item50.ReadFromBytes(data.Slice(400, 8), mode);
    this._item51.ReadFromBytes(data.Slice(408, 8), mode);
    this._item52.ReadFromBytes(data.Slice(416, 8), mode);
    this._item53.ReadFromBytes(data.Slice(424, 8), mode);
    this._item54.ReadFromBytes(data.Slice(432, 8), mode);
    this._item55.ReadFromBytes(data.Slice(440, 8), mode);
    this._item56.ReadFromBytes(data.Slice(448, 8), mode);
    this._item57.ReadFromBytes(data.Slice(456, 8), mode);
    this._item58.ReadFromBytes(data.Slice(464, 8), mode);
    this._item59.ReadFromBytes(data.Slice(472, 8), mode);
    this._item60.ReadFromBytes(data.Slice(480, 8), mode);
    this._item61.ReadFromBytes(data.Slice(488, 8), mode);
    this._item62.ReadFromBytes(data.Slice(496, 8), mode);
    this._item63.ReadFromBytes(data.Slice(504, 8), mode);
  }

  public int Length => 64;
  public int Count => 64;

  public ref long this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<long> AsSpan()
  {
    return CreateSpan(ref _item0, 64);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<long> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
