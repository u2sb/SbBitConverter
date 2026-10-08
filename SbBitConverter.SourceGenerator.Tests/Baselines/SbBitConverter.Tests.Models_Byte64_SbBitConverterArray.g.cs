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
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 64)]
partial struct Byte64
{
  public Byte64(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Byte64>());
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
    this._item16 = data[16];
    this._item17 = data[17];
    this._item18 = data[18];
    this._item19 = data[19];
    this._item20 = data[20];
    this._item21 = data[21];
    this._item22 = data[22];
    this._item23 = data[23];
    this._item24 = data[24];
    this._item25 = data[25];
    this._item26 = data[26];
    this._item27 = data[27];
    this._item28 = data[28];
    this._item29 = data[29];
    this._item30 = data[30];
    this._item31 = data[31];
    this._item32 = data[32];
    this._item33 = data[33];
    this._item34 = data[34];
    this._item35 = data[35];
    this._item36 = data[36];
    this._item37 = data[37];
    this._item38 = data[38];
    this._item39 = data[39];
    this._item40 = data[40];
    this._item41 = data[41];
    this._item42 = data[42];
    this._item43 = data[43];
    this._item44 = data[44];
    this._item45 = data[45];
    this._item46 = data[46];
    this._item47 = data[47];
    this._item48 = data[48];
    this._item49 = data[49];
    this._item50 = data[50];
    this._item51 = data[51];
    this._item52 = data[52];
    this._item53 = data[53];
    this._item54 = data[54];
    this._item55 = data[55];
    this._item56 = data[56];
    this._item57 = data[57];
    this._item58 = data[58];
    this._item59 = data[59];
    this._item60 = data[60];
    this._item61 = data[61];
    this._item62 = data[62];
    this._item63 = data[63];
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

  [FieldOffset(16)]private byte _item16;

  [FieldOffset(17)]private byte _item17;

  [FieldOffset(18)]private byte _item18;

  [FieldOffset(19)]private byte _item19;

  [FieldOffset(20)]private byte _item20;

  [FieldOffset(21)]private byte _item21;

  [FieldOffset(22)]private byte _item22;

  [FieldOffset(23)]private byte _item23;

  [FieldOffset(24)]private byte _item24;

  [FieldOffset(25)]private byte _item25;

  [FieldOffset(26)]private byte _item26;

  [FieldOffset(27)]private byte _item27;

  [FieldOffset(28)]private byte _item28;

  [FieldOffset(29)]private byte _item29;

  [FieldOffset(30)]private byte _item30;

  [FieldOffset(31)]private byte _item31;

  [FieldOffset(32)]private byte _item32;

  [FieldOffset(33)]private byte _item33;

  [FieldOffset(34)]private byte _item34;

  [FieldOffset(35)]private byte _item35;

  [FieldOffset(36)]private byte _item36;

  [FieldOffset(37)]private byte _item37;

  [FieldOffset(38)]private byte _item38;

  [FieldOffset(39)]private byte _item39;

  [FieldOffset(40)]private byte _item40;

  [FieldOffset(41)]private byte _item41;

  [FieldOffset(42)]private byte _item42;

  [FieldOffset(43)]private byte _item43;

  [FieldOffset(44)]private byte _item44;

  [FieldOffset(45)]private byte _item45;

  [FieldOffset(46)]private byte _item46;

  [FieldOffset(47)]private byte _item47;

  [FieldOffset(48)]private byte _item48;

  [FieldOffset(49)]private byte _item49;

  [FieldOffset(50)]private byte _item50;

  [FieldOffset(51)]private byte _item51;

  [FieldOffset(52)]private byte _item52;

  [FieldOffset(53)]private byte _item53;

  [FieldOffset(54)]private byte _item54;

  [FieldOffset(55)]private byte _item55;

  [FieldOffset(56)]private byte _item56;

  [FieldOffset(57)]private byte _item57;

  [FieldOffset(58)]private byte _item58;

  [FieldOffset(59)]private byte _item59;

  [FieldOffset(60)]private byte _item60;

  [FieldOffset(61)]private byte _item61;

  [FieldOffset(62)]private byte _item62;

  [FieldOffset(63)]private byte _item63;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Byte64>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Byte64>());
    AsSpan().CopyTo(span);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Byte64>());
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
    this._item16 = data[16];
    this._item17 = data[17];
    this._item18 = data[18];
    this._item19 = data[19];
    this._item20 = data[20];
    this._item21 = data[21];
    this._item22 = data[22];
    this._item23 = data[23];
    this._item24 = data[24];
    this._item25 = data[25];
    this._item26 = data[26];
    this._item27 = data[27];
    this._item28 = data[28];
    this._item29 = data[29];
    this._item30 = data[30];
    this._item31 = data[31];
    this._item32 = data[32];
    this._item33 = data[33];
    this._item34 = data[34];
    this._item35 = data[35];
    this._item36 = data[36];
    this._item37 = data[37];
    this._item38 = data[38];
    this._item39 = data[39];
    this._item40 = data[40];
    this._item41 = data[41];
    this._item42 = data[42];
    this._item43 = data[43];
    this._item44 = data[44];
    this._item45 = data[45];
    this._item46 = data[46];
    this._item47 = data[47];
    this._item48 = data[48];
    this._item49 = data[49];
    this._item50 = data[50];
    this._item51 = data[51];
    this._item52 = data[52];
    this._item53 = data[53];
    this._item54 = data[54];
    this._item55 = data[55];
    this._item56 = data[56];
    this._item57 = data[57];
    this._item58 = data[58];
    this._item59 = data[59];
    this._item60 = data[60];
    this._item61 = data[61];
    this._item62 = data[62];
    this._item63 = data[63];
  }

  public int Length => 64;
  public int Count => 64;

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
    return CreateSpan(ref _item0, 64);
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
