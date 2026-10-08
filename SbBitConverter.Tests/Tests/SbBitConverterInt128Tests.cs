// 补充覆盖：对应 Sb.Extensions\System\SbBitConverter\SbBitConverter.Int128.cs
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   Int128 / UInt128 的 WithEndianness、ApplyEndianness、AsReadOnlyByteSpan 测试
/// </summary>
public class SbBitConverterInt128Tests
{
  // ── Int128.WithEndianness ──

  [Fact]
  public void Int128_WithEndianness_LittleEndianMode_ReturnsSameValue()
  {
    Int128 value = new(0x1122334455667788, 0x99AABBCCDDEEFF00);
    Assert.Equal(value, value.WithEndianness(false));
  }

  [Fact]
  public void Int128_WithEndianness_BigEndianMode_ReversesBytes()
  {
    Int128 value = new(0x1122334455667788, 0x99AABBCCDDEEFF00);
    var expected = BinaryPrimitives.ReverseEndianness(value);
    Assert.Equal(expected, value.WithEndianness(true));
  }

  [Fact]
  public void Int128_WithEndianness_BigEndian_Twice_IsIdentity()
  {
    Int128 value = new(0x0123456789ABCDEF, 0xFEDCBA9876543210);
    Assert.Equal(value, value.WithEndianness(true).WithEndianness(true));
  }

  [Fact]
  public void Int128_WithEndianness_Zero_IsZero()
  {
    Assert.Equal(Int128.Zero, Int128.Zero.WithEndianness(true));
    Assert.Equal(Int128.Zero, Int128.Zero.WithEndianness(false));
  }

  // ── Int128.ApplyEndianness ──

  [Fact]
  public void Int128_ApplyEndianness_MutatesInPlace()
  {
    var value = new Int128(0x0807060504030201UL, 0x100F0E0D0C0B0A09UL); // 字节 01..10
    var expected = BinaryPrimitives.ReverseEndianness(value);
    value.ApplyEndianness(true);
    Assert.Equal(expected, value);
  }

  [Fact]
  public void Int128_ApplyEndianness_LittleEndianMode_NoChange()
  {
    var value = (Int128)12345;
    value.ApplyEndianness(false);
    Assert.Equal((Int128)12345, value);
  }

  // ── Int128.AsReadOnlyByteSpan ──

  [Fact]
  public void Int128_AsReadOnlyByteSpan_Length16_LittleEndianLayout()
  {
    var value = new Int128(0, 0x0807060504030201UL); // 低位 8 字节为 01..08
    ReadOnlySpan<byte> bytes = value.AsReadOnlyByteSpan();

    Assert.Equal(16, bytes.Length);
    for (var i = 0; i < 8; i++) Assert.Equal((byte)(i + 1), bytes[i]);
    for (var i = 8; i < 16; i++) Assert.Equal(0, bytes[i]);
  }

  [Fact]
  public void Int128_AsReadOnlyByteSpan_RoundTrip()
  {
    var value = Int128.CreateTruncating(0xDEADBEEFCAFEBABEUL);
    ReadOnlySpan<byte> bytes = value.AsReadOnlyByteSpan();
    Assert.Equal(value, MemoryMarshal.Read<Int128>(bytes));
  }

  [Fact]
  public void Int128_NegativeValue_AsReadOnlyByteSpan_HasHighBitsSet()
  {
    var value = Int128.MinValue;
    ReadOnlySpan<byte> bytes = value.AsReadOnlyByteSpan();
    Assert.Equal(16, bytes.Length);
    Assert.Equal(0, bytes[0]); // 低位字节为 0
    Assert.Equal(0x80, bytes[15]); // 符号位在最高字节
  }

  // ── UInt128.WithEndianness ──

  [Fact]
  public void UInt128_WithEndianness_LittleEndianMode_ReturnsSameValue()
  {
    UInt128 value = new(0x1122334455667788, 0x99AABBCCDDEEFF00);
    Assert.Equal(value, value.WithEndianness(false));
  }

  [Fact]
  public void UInt128_WithEndianness_BigEndianMode_ReversesBytes()
  {
    UInt128 value = new(0x1122334455667788, 0x99AABBCCDDEEFF00);
    var expected = BinaryPrimitives.ReverseEndianness(value);
    Assert.Equal(expected, value.WithEndianness(true));
  }

  [Fact]
  public void UInt128_WithEndianness_MaxValue_BigEndian_RoundTrips()
  {
    var swapped = UInt128.MaxValue.WithEndianness(true);
    Assert.Equal(UInt128.MaxValue, swapped.WithEndianness(true));
  }

  [Fact]
  public void UInt128_WithEndianness_One_BecomesHighByte()
  {
    // 小端机器上 1 的字节序为 01 00 ... 00，翻转后为 00 ... 01
    Assert.Equal(UInt128.One, UInt128.One.WithEndianness(false));
    Assert.Equal(UInt128.One << 120, UInt128.One.WithEndianness(true));
  }

  // ── UInt128.ApplyEndianness ──

  [Fact]
  public void UInt128_ApplyEndianness_MutatesInPlace()
  {
    var value = new Int128(0x0102030405060708UL, 0x090A0B0C0D0E0F10UL);
    var expected = BinaryPrimitives.ReverseEndianness(value);
    value.ApplyEndianness(true);
    Assert.Equal(expected, value);
  }

  [Fact]
  public void UInt128_ApplyEndianness_LittleEndianMode_NoChange()
  {
    var value = (UInt128)0xABCD;
    value.ApplyEndianness(false);
    Assert.Equal((UInt128)0xABCD, value);
  }

  // ── UInt128.AsReadOnlyByteSpan ──

  [Fact]
  public void UInt128_AsReadOnlyByteSpan_Length16_LittleEndianLayout()
  {
    var value = new UInt128(0, 0x0807060504030201UL); // 低位 8 字节为 01..08
    ReadOnlySpan<byte> bytes = value.AsReadOnlyByteSpan();

    Assert.Equal(16, bytes.Length);
    for (var i = 0; i < 8; i++) Assert.Equal((byte)(i + 1), bytes[i]);
    for (var i = 8; i < 16; i++) Assert.Equal(0, bytes[i]);
  }

  [Fact]
  public void UInt128_AsReadOnlyByteSpan_RoundTrip()
  {
    var value = UInt128.CreateTruncating(0xFEEDFACEUL);
    ReadOnlySpan<byte> bytes = value.AsReadOnlyByteSpan();
    Assert.Equal(value, MemoryMarshal.Read<UInt128>(bytes));
  }
}
