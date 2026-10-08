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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 512)]
partial struct Int128Arr
{
  public Int128Arr(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int128Arr>());
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
    this._item32 = data.Slice(128, 4).ToT<int>(mode);
    this._item33 = data.Slice(132, 4).ToT<int>(mode);
    this._item34 = data.Slice(136, 4).ToT<int>(mode);
    this._item35 = data.Slice(140, 4).ToT<int>(mode);
    this._item36 = data.Slice(144, 4).ToT<int>(mode);
    this._item37 = data.Slice(148, 4).ToT<int>(mode);
    this._item38 = data.Slice(152, 4).ToT<int>(mode);
    this._item39 = data.Slice(156, 4).ToT<int>(mode);
    this._item40 = data.Slice(160, 4).ToT<int>(mode);
    this._item41 = data.Slice(164, 4).ToT<int>(mode);
    this._item42 = data.Slice(168, 4).ToT<int>(mode);
    this._item43 = data.Slice(172, 4).ToT<int>(mode);
    this._item44 = data.Slice(176, 4).ToT<int>(mode);
    this._item45 = data.Slice(180, 4).ToT<int>(mode);
    this._item46 = data.Slice(184, 4).ToT<int>(mode);
    this._item47 = data.Slice(188, 4).ToT<int>(mode);
    this._item48 = data.Slice(192, 4).ToT<int>(mode);
    this._item49 = data.Slice(196, 4).ToT<int>(mode);
    this._item50 = data.Slice(200, 4).ToT<int>(mode);
    this._item51 = data.Slice(204, 4).ToT<int>(mode);
    this._item52 = data.Slice(208, 4).ToT<int>(mode);
    this._item53 = data.Slice(212, 4).ToT<int>(mode);
    this._item54 = data.Slice(216, 4).ToT<int>(mode);
    this._item55 = data.Slice(220, 4).ToT<int>(mode);
    this._item56 = data.Slice(224, 4).ToT<int>(mode);
    this._item57 = data.Slice(228, 4).ToT<int>(mode);
    this._item58 = data.Slice(232, 4).ToT<int>(mode);
    this._item59 = data.Slice(236, 4).ToT<int>(mode);
    this._item60 = data.Slice(240, 4).ToT<int>(mode);
    this._item61 = data.Slice(244, 4).ToT<int>(mode);
    this._item62 = data.Slice(248, 4).ToT<int>(mode);
    this._item63 = data.Slice(252, 4).ToT<int>(mode);
    this._item64 = data.Slice(256, 4).ToT<int>(mode);
    this._item65 = data.Slice(260, 4).ToT<int>(mode);
    this._item66 = data.Slice(264, 4).ToT<int>(mode);
    this._item67 = data.Slice(268, 4).ToT<int>(mode);
    this._item68 = data.Slice(272, 4).ToT<int>(mode);
    this._item69 = data.Slice(276, 4).ToT<int>(mode);
    this._item70 = data.Slice(280, 4).ToT<int>(mode);
    this._item71 = data.Slice(284, 4).ToT<int>(mode);
    this._item72 = data.Slice(288, 4).ToT<int>(mode);
    this._item73 = data.Slice(292, 4).ToT<int>(mode);
    this._item74 = data.Slice(296, 4).ToT<int>(mode);
    this._item75 = data.Slice(300, 4).ToT<int>(mode);
    this._item76 = data.Slice(304, 4).ToT<int>(mode);
    this._item77 = data.Slice(308, 4).ToT<int>(mode);
    this._item78 = data.Slice(312, 4).ToT<int>(mode);
    this._item79 = data.Slice(316, 4).ToT<int>(mode);
    this._item80 = data.Slice(320, 4).ToT<int>(mode);
    this._item81 = data.Slice(324, 4).ToT<int>(mode);
    this._item82 = data.Slice(328, 4).ToT<int>(mode);
    this._item83 = data.Slice(332, 4).ToT<int>(mode);
    this._item84 = data.Slice(336, 4).ToT<int>(mode);
    this._item85 = data.Slice(340, 4).ToT<int>(mode);
    this._item86 = data.Slice(344, 4).ToT<int>(mode);
    this._item87 = data.Slice(348, 4).ToT<int>(mode);
    this._item88 = data.Slice(352, 4).ToT<int>(mode);
    this._item89 = data.Slice(356, 4).ToT<int>(mode);
    this._item90 = data.Slice(360, 4).ToT<int>(mode);
    this._item91 = data.Slice(364, 4).ToT<int>(mode);
    this._item92 = data.Slice(368, 4).ToT<int>(mode);
    this._item93 = data.Slice(372, 4).ToT<int>(mode);
    this._item94 = data.Slice(376, 4).ToT<int>(mode);
    this._item95 = data.Slice(380, 4).ToT<int>(mode);
    this._item96 = data.Slice(384, 4).ToT<int>(mode);
    this._item97 = data.Slice(388, 4).ToT<int>(mode);
    this._item98 = data.Slice(392, 4).ToT<int>(mode);
    this._item99 = data.Slice(396, 4).ToT<int>(mode);
    this._item100 = data.Slice(400, 4).ToT<int>(mode);
    this._item101 = data.Slice(404, 4).ToT<int>(mode);
    this._item102 = data.Slice(408, 4).ToT<int>(mode);
    this._item103 = data.Slice(412, 4).ToT<int>(mode);
    this._item104 = data.Slice(416, 4).ToT<int>(mode);
    this._item105 = data.Slice(420, 4).ToT<int>(mode);
    this._item106 = data.Slice(424, 4).ToT<int>(mode);
    this._item107 = data.Slice(428, 4).ToT<int>(mode);
    this._item108 = data.Slice(432, 4).ToT<int>(mode);
    this._item109 = data.Slice(436, 4).ToT<int>(mode);
    this._item110 = data.Slice(440, 4).ToT<int>(mode);
    this._item111 = data.Slice(444, 4).ToT<int>(mode);
    this._item112 = data.Slice(448, 4).ToT<int>(mode);
    this._item113 = data.Slice(452, 4).ToT<int>(mode);
    this._item114 = data.Slice(456, 4).ToT<int>(mode);
    this._item115 = data.Slice(460, 4).ToT<int>(mode);
    this._item116 = data.Slice(464, 4).ToT<int>(mode);
    this._item117 = data.Slice(468, 4).ToT<int>(mode);
    this._item118 = data.Slice(472, 4).ToT<int>(mode);
    this._item119 = data.Slice(476, 4).ToT<int>(mode);
    this._item120 = data.Slice(480, 4).ToT<int>(mode);
    this._item121 = data.Slice(484, 4).ToT<int>(mode);
    this._item122 = data.Slice(488, 4).ToT<int>(mode);
    this._item123 = data.Slice(492, 4).ToT<int>(mode);
    this._item124 = data.Slice(496, 4).ToT<int>(mode);
    this._item125 = data.Slice(500, 4).ToT<int>(mode);
    this._item126 = data.Slice(504, 4).ToT<int>(mode);
    this._item127 = data.Slice(508, 4).ToT<int>(mode);
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

  [FieldOffset(128)]private int _item32;

  [FieldOffset(132)]private int _item33;

  [FieldOffset(136)]private int _item34;

  [FieldOffset(140)]private int _item35;

  [FieldOffset(144)]private int _item36;

  [FieldOffset(148)]private int _item37;

  [FieldOffset(152)]private int _item38;

  [FieldOffset(156)]private int _item39;

  [FieldOffset(160)]private int _item40;

  [FieldOffset(164)]private int _item41;

  [FieldOffset(168)]private int _item42;

  [FieldOffset(172)]private int _item43;

  [FieldOffset(176)]private int _item44;

  [FieldOffset(180)]private int _item45;

  [FieldOffset(184)]private int _item46;

  [FieldOffset(188)]private int _item47;

  [FieldOffset(192)]private int _item48;

  [FieldOffset(196)]private int _item49;

  [FieldOffset(200)]private int _item50;

  [FieldOffset(204)]private int _item51;

  [FieldOffset(208)]private int _item52;

  [FieldOffset(212)]private int _item53;

  [FieldOffset(216)]private int _item54;

  [FieldOffset(220)]private int _item55;

  [FieldOffset(224)]private int _item56;

  [FieldOffset(228)]private int _item57;

  [FieldOffset(232)]private int _item58;

  [FieldOffset(236)]private int _item59;

  [FieldOffset(240)]private int _item60;

  [FieldOffset(244)]private int _item61;

  [FieldOffset(248)]private int _item62;

  [FieldOffset(252)]private int _item63;

  [FieldOffset(256)]private int _item64;

  [FieldOffset(260)]private int _item65;

  [FieldOffset(264)]private int _item66;

  [FieldOffset(268)]private int _item67;

  [FieldOffset(272)]private int _item68;

  [FieldOffset(276)]private int _item69;

  [FieldOffset(280)]private int _item70;

  [FieldOffset(284)]private int _item71;

  [FieldOffset(288)]private int _item72;

  [FieldOffset(292)]private int _item73;

  [FieldOffset(296)]private int _item74;

  [FieldOffset(300)]private int _item75;

  [FieldOffset(304)]private int _item76;

  [FieldOffset(308)]private int _item77;

  [FieldOffset(312)]private int _item78;

  [FieldOffset(316)]private int _item79;

  [FieldOffset(320)]private int _item80;

  [FieldOffset(324)]private int _item81;

  [FieldOffset(328)]private int _item82;

  [FieldOffset(332)]private int _item83;

  [FieldOffset(336)]private int _item84;

  [FieldOffset(340)]private int _item85;

  [FieldOffset(344)]private int _item86;

  [FieldOffset(348)]private int _item87;

  [FieldOffset(352)]private int _item88;

  [FieldOffset(356)]private int _item89;

  [FieldOffset(360)]private int _item90;

  [FieldOffset(364)]private int _item91;

  [FieldOffset(368)]private int _item92;

  [FieldOffset(372)]private int _item93;

  [FieldOffset(376)]private int _item94;

  [FieldOffset(380)]private int _item95;

  [FieldOffset(384)]private int _item96;

  [FieldOffset(388)]private int _item97;

  [FieldOffset(392)]private int _item98;

  [FieldOffset(396)]private int _item99;

  [FieldOffset(400)]private int _item100;

  [FieldOffset(404)]private int _item101;

  [FieldOffset(408)]private int _item102;

  [FieldOffset(412)]private int _item103;

  [FieldOffset(416)]private int _item104;

  [FieldOffset(420)]private int _item105;

  [FieldOffset(424)]private int _item106;

  [FieldOffset(428)]private int _item107;

  [FieldOffset(432)]private int _item108;

  [FieldOffset(436)]private int _item109;

  [FieldOffset(440)]private int _item110;

  [FieldOffset(444)]private int _item111;

  [FieldOffset(448)]private int _item112;

  [FieldOffset(452)]private int _item113;

  [FieldOffset(456)]private int _item114;

  [FieldOffset(460)]private int _item115;

  [FieldOffset(464)]private int _item116;

  [FieldOffset(468)]private int _item117;

  [FieldOffset(472)]private int _item118;

  [FieldOffset(476)]private int _item119;

  [FieldOffset(480)]private int _item120;

  [FieldOffset(484)]private int _item121;

  [FieldOffset(488)]private int _item122;

  [FieldOffset(492)]private int _item123;

  [FieldOffset(496)]private int _item124;

  [FieldOffset(500)]private int _item125;

  [FieldOffset(504)]private int _item126;

  [FieldOffset(508)]private int _item127;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Int128Arr>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Int128Arr>());
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
    this._item32.WriteTo<int>(span.Slice(128, 4), mode);
    this._item33.WriteTo<int>(span.Slice(132, 4), mode);
    this._item34.WriteTo<int>(span.Slice(136, 4), mode);
    this._item35.WriteTo<int>(span.Slice(140, 4), mode);
    this._item36.WriteTo<int>(span.Slice(144, 4), mode);
    this._item37.WriteTo<int>(span.Slice(148, 4), mode);
    this._item38.WriteTo<int>(span.Slice(152, 4), mode);
    this._item39.WriteTo<int>(span.Slice(156, 4), mode);
    this._item40.WriteTo<int>(span.Slice(160, 4), mode);
    this._item41.WriteTo<int>(span.Slice(164, 4), mode);
    this._item42.WriteTo<int>(span.Slice(168, 4), mode);
    this._item43.WriteTo<int>(span.Slice(172, 4), mode);
    this._item44.WriteTo<int>(span.Slice(176, 4), mode);
    this._item45.WriteTo<int>(span.Slice(180, 4), mode);
    this._item46.WriteTo<int>(span.Slice(184, 4), mode);
    this._item47.WriteTo<int>(span.Slice(188, 4), mode);
    this._item48.WriteTo<int>(span.Slice(192, 4), mode);
    this._item49.WriteTo<int>(span.Slice(196, 4), mode);
    this._item50.WriteTo<int>(span.Slice(200, 4), mode);
    this._item51.WriteTo<int>(span.Slice(204, 4), mode);
    this._item52.WriteTo<int>(span.Slice(208, 4), mode);
    this._item53.WriteTo<int>(span.Slice(212, 4), mode);
    this._item54.WriteTo<int>(span.Slice(216, 4), mode);
    this._item55.WriteTo<int>(span.Slice(220, 4), mode);
    this._item56.WriteTo<int>(span.Slice(224, 4), mode);
    this._item57.WriteTo<int>(span.Slice(228, 4), mode);
    this._item58.WriteTo<int>(span.Slice(232, 4), mode);
    this._item59.WriteTo<int>(span.Slice(236, 4), mode);
    this._item60.WriteTo<int>(span.Slice(240, 4), mode);
    this._item61.WriteTo<int>(span.Slice(244, 4), mode);
    this._item62.WriteTo<int>(span.Slice(248, 4), mode);
    this._item63.WriteTo<int>(span.Slice(252, 4), mode);
    this._item64.WriteTo<int>(span.Slice(256, 4), mode);
    this._item65.WriteTo<int>(span.Slice(260, 4), mode);
    this._item66.WriteTo<int>(span.Slice(264, 4), mode);
    this._item67.WriteTo<int>(span.Slice(268, 4), mode);
    this._item68.WriteTo<int>(span.Slice(272, 4), mode);
    this._item69.WriteTo<int>(span.Slice(276, 4), mode);
    this._item70.WriteTo<int>(span.Slice(280, 4), mode);
    this._item71.WriteTo<int>(span.Slice(284, 4), mode);
    this._item72.WriteTo<int>(span.Slice(288, 4), mode);
    this._item73.WriteTo<int>(span.Slice(292, 4), mode);
    this._item74.WriteTo<int>(span.Slice(296, 4), mode);
    this._item75.WriteTo<int>(span.Slice(300, 4), mode);
    this._item76.WriteTo<int>(span.Slice(304, 4), mode);
    this._item77.WriteTo<int>(span.Slice(308, 4), mode);
    this._item78.WriteTo<int>(span.Slice(312, 4), mode);
    this._item79.WriteTo<int>(span.Slice(316, 4), mode);
    this._item80.WriteTo<int>(span.Slice(320, 4), mode);
    this._item81.WriteTo<int>(span.Slice(324, 4), mode);
    this._item82.WriteTo<int>(span.Slice(328, 4), mode);
    this._item83.WriteTo<int>(span.Slice(332, 4), mode);
    this._item84.WriteTo<int>(span.Slice(336, 4), mode);
    this._item85.WriteTo<int>(span.Slice(340, 4), mode);
    this._item86.WriteTo<int>(span.Slice(344, 4), mode);
    this._item87.WriteTo<int>(span.Slice(348, 4), mode);
    this._item88.WriteTo<int>(span.Slice(352, 4), mode);
    this._item89.WriteTo<int>(span.Slice(356, 4), mode);
    this._item90.WriteTo<int>(span.Slice(360, 4), mode);
    this._item91.WriteTo<int>(span.Slice(364, 4), mode);
    this._item92.WriteTo<int>(span.Slice(368, 4), mode);
    this._item93.WriteTo<int>(span.Slice(372, 4), mode);
    this._item94.WriteTo<int>(span.Slice(376, 4), mode);
    this._item95.WriteTo<int>(span.Slice(380, 4), mode);
    this._item96.WriteTo<int>(span.Slice(384, 4), mode);
    this._item97.WriteTo<int>(span.Slice(388, 4), mode);
    this._item98.WriteTo<int>(span.Slice(392, 4), mode);
    this._item99.WriteTo<int>(span.Slice(396, 4), mode);
    this._item100.WriteTo<int>(span.Slice(400, 4), mode);
    this._item101.WriteTo<int>(span.Slice(404, 4), mode);
    this._item102.WriteTo<int>(span.Slice(408, 4), mode);
    this._item103.WriteTo<int>(span.Slice(412, 4), mode);
    this._item104.WriteTo<int>(span.Slice(416, 4), mode);
    this._item105.WriteTo<int>(span.Slice(420, 4), mode);
    this._item106.WriteTo<int>(span.Slice(424, 4), mode);
    this._item107.WriteTo<int>(span.Slice(428, 4), mode);
    this._item108.WriteTo<int>(span.Slice(432, 4), mode);
    this._item109.WriteTo<int>(span.Slice(436, 4), mode);
    this._item110.WriteTo<int>(span.Slice(440, 4), mode);
    this._item111.WriteTo<int>(span.Slice(444, 4), mode);
    this._item112.WriteTo<int>(span.Slice(448, 4), mode);
    this._item113.WriteTo<int>(span.Slice(452, 4), mode);
    this._item114.WriteTo<int>(span.Slice(456, 4), mode);
    this._item115.WriteTo<int>(span.Slice(460, 4), mode);
    this._item116.WriteTo<int>(span.Slice(464, 4), mode);
    this._item117.WriteTo<int>(span.Slice(468, 4), mode);
    this._item118.WriteTo<int>(span.Slice(472, 4), mode);
    this._item119.WriteTo<int>(span.Slice(476, 4), mode);
    this._item120.WriteTo<int>(span.Slice(480, 4), mode);
    this._item121.WriteTo<int>(span.Slice(484, 4), mode);
    this._item122.WriteTo<int>(span.Slice(488, 4), mode);
    this._item123.WriteTo<int>(span.Slice(492, 4), mode);
    this._item124.WriteTo<int>(span.Slice(496, 4), mode);
    this._item125.WriteTo<int>(span.Slice(500, 4), mode);
    this._item126.WriteTo<int>(span.Slice(504, 4), mode);
    this._item127.WriteTo<int>(span.Slice(508, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int128Arr>());
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
    this._item32.ReadFromBytes(data.Slice(128, 4), mode);
    this._item33.ReadFromBytes(data.Slice(132, 4), mode);
    this._item34.ReadFromBytes(data.Slice(136, 4), mode);
    this._item35.ReadFromBytes(data.Slice(140, 4), mode);
    this._item36.ReadFromBytes(data.Slice(144, 4), mode);
    this._item37.ReadFromBytes(data.Slice(148, 4), mode);
    this._item38.ReadFromBytes(data.Slice(152, 4), mode);
    this._item39.ReadFromBytes(data.Slice(156, 4), mode);
    this._item40.ReadFromBytes(data.Slice(160, 4), mode);
    this._item41.ReadFromBytes(data.Slice(164, 4), mode);
    this._item42.ReadFromBytes(data.Slice(168, 4), mode);
    this._item43.ReadFromBytes(data.Slice(172, 4), mode);
    this._item44.ReadFromBytes(data.Slice(176, 4), mode);
    this._item45.ReadFromBytes(data.Slice(180, 4), mode);
    this._item46.ReadFromBytes(data.Slice(184, 4), mode);
    this._item47.ReadFromBytes(data.Slice(188, 4), mode);
    this._item48.ReadFromBytes(data.Slice(192, 4), mode);
    this._item49.ReadFromBytes(data.Slice(196, 4), mode);
    this._item50.ReadFromBytes(data.Slice(200, 4), mode);
    this._item51.ReadFromBytes(data.Slice(204, 4), mode);
    this._item52.ReadFromBytes(data.Slice(208, 4), mode);
    this._item53.ReadFromBytes(data.Slice(212, 4), mode);
    this._item54.ReadFromBytes(data.Slice(216, 4), mode);
    this._item55.ReadFromBytes(data.Slice(220, 4), mode);
    this._item56.ReadFromBytes(data.Slice(224, 4), mode);
    this._item57.ReadFromBytes(data.Slice(228, 4), mode);
    this._item58.ReadFromBytes(data.Slice(232, 4), mode);
    this._item59.ReadFromBytes(data.Slice(236, 4), mode);
    this._item60.ReadFromBytes(data.Slice(240, 4), mode);
    this._item61.ReadFromBytes(data.Slice(244, 4), mode);
    this._item62.ReadFromBytes(data.Slice(248, 4), mode);
    this._item63.ReadFromBytes(data.Slice(252, 4), mode);
    this._item64.ReadFromBytes(data.Slice(256, 4), mode);
    this._item65.ReadFromBytes(data.Slice(260, 4), mode);
    this._item66.ReadFromBytes(data.Slice(264, 4), mode);
    this._item67.ReadFromBytes(data.Slice(268, 4), mode);
    this._item68.ReadFromBytes(data.Slice(272, 4), mode);
    this._item69.ReadFromBytes(data.Slice(276, 4), mode);
    this._item70.ReadFromBytes(data.Slice(280, 4), mode);
    this._item71.ReadFromBytes(data.Slice(284, 4), mode);
    this._item72.ReadFromBytes(data.Slice(288, 4), mode);
    this._item73.ReadFromBytes(data.Slice(292, 4), mode);
    this._item74.ReadFromBytes(data.Slice(296, 4), mode);
    this._item75.ReadFromBytes(data.Slice(300, 4), mode);
    this._item76.ReadFromBytes(data.Slice(304, 4), mode);
    this._item77.ReadFromBytes(data.Slice(308, 4), mode);
    this._item78.ReadFromBytes(data.Slice(312, 4), mode);
    this._item79.ReadFromBytes(data.Slice(316, 4), mode);
    this._item80.ReadFromBytes(data.Slice(320, 4), mode);
    this._item81.ReadFromBytes(data.Slice(324, 4), mode);
    this._item82.ReadFromBytes(data.Slice(328, 4), mode);
    this._item83.ReadFromBytes(data.Slice(332, 4), mode);
    this._item84.ReadFromBytes(data.Slice(336, 4), mode);
    this._item85.ReadFromBytes(data.Slice(340, 4), mode);
    this._item86.ReadFromBytes(data.Slice(344, 4), mode);
    this._item87.ReadFromBytes(data.Slice(348, 4), mode);
    this._item88.ReadFromBytes(data.Slice(352, 4), mode);
    this._item89.ReadFromBytes(data.Slice(356, 4), mode);
    this._item90.ReadFromBytes(data.Slice(360, 4), mode);
    this._item91.ReadFromBytes(data.Slice(364, 4), mode);
    this._item92.ReadFromBytes(data.Slice(368, 4), mode);
    this._item93.ReadFromBytes(data.Slice(372, 4), mode);
    this._item94.ReadFromBytes(data.Slice(376, 4), mode);
    this._item95.ReadFromBytes(data.Slice(380, 4), mode);
    this._item96.ReadFromBytes(data.Slice(384, 4), mode);
    this._item97.ReadFromBytes(data.Slice(388, 4), mode);
    this._item98.ReadFromBytes(data.Slice(392, 4), mode);
    this._item99.ReadFromBytes(data.Slice(396, 4), mode);
    this._item100.ReadFromBytes(data.Slice(400, 4), mode);
    this._item101.ReadFromBytes(data.Slice(404, 4), mode);
    this._item102.ReadFromBytes(data.Slice(408, 4), mode);
    this._item103.ReadFromBytes(data.Slice(412, 4), mode);
    this._item104.ReadFromBytes(data.Slice(416, 4), mode);
    this._item105.ReadFromBytes(data.Slice(420, 4), mode);
    this._item106.ReadFromBytes(data.Slice(424, 4), mode);
    this._item107.ReadFromBytes(data.Slice(428, 4), mode);
    this._item108.ReadFromBytes(data.Slice(432, 4), mode);
    this._item109.ReadFromBytes(data.Slice(436, 4), mode);
    this._item110.ReadFromBytes(data.Slice(440, 4), mode);
    this._item111.ReadFromBytes(data.Slice(444, 4), mode);
    this._item112.ReadFromBytes(data.Slice(448, 4), mode);
    this._item113.ReadFromBytes(data.Slice(452, 4), mode);
    this._item114.ReadFromBytes(data.Slice(456, 4), mode);
    this._item115.ReadFromBytes(data.Slice(460, 4), mode);
    this._item116.ReadFromBytes(data.Slice(464, 4), mode);
    this._item117.ReadFromBytes(data.Slice(468, 4), mode);
    this._item118.ReadFromBytes(data.Slice(472, 4), mode);
    this._item119.ReadFromBytes(data.Slice(476, 4), mode);
    this._item120.ReadFromBytes(data.Slice(480, 4), mode);
    this._item121.ReadFromBytes(data.Slice(484, 4), mode);
    this._item122.ReadFromBytes(data.Slice(488, 4), mode);
    this._item123.ReadFromBytes(data.Slice(492, 4), mode);
    this._item124.ReadFromBytes(data.Slice(496, 4), mode);
    this._item125.ReadFromBytes(data.Slice(500, 4), mode);
    this._item126.ReadFromBytes(data.Slice(504, 4), mode);
    this._item127.ReadFromBytes(data.Slice(508, 4), mode);
  }

  public int Length => 128;
  public int Count => 128;

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
    return CreateSpan(ref _item0, 128);
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
