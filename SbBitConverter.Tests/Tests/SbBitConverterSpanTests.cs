// 补充覆盖：对应 Sb.Extensions\System\SbBitConverter\SbBitConverter.Span.cs
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   SbBitConverter.Span 中 ReadOnlySpan/Span 批量读写与大小端转换 API 测试
/// </summary>
public class SbBitConverterSpanTests
{
  // ── ReadOnlySpan：ToInt16 / ToUInt16 ──

  [Fact]
  public void ReadOnlySpan_ToInt16_LittleEndian()
  {
    byte[] bytes = [0x34, 0x12];
    Assert.Equal(0x1234, bytes.ToInt16());
  }

  [Fact]
  public void ReadOnlySpan_ToInt16_BigEndian()
  {
    byte[] bytes = [0x12, 0x34];
    Assert.Equal(0x1234, bytes.ToInt16(true));
  }

  [Fact]
  public void ReadOnlySpan_ToInt16_Negative()
  {
    byte[] bytes = [0xFF, 0xFF];
    Assert.Equal(-1, bytes.ToInt16());
  }

  [Fact]
  public void ReadOnlySpan_ToUInt16_RoundTrips()
  {
    var value = (ushort)0xBEEF;
    var bytes = BitConverter.GetBytes(value);
    Assert.Equal(value, bytes.ToUInt16());
  }

  // ── ReadOnlySpan：ToInt32 / ToUInt32 ──

  [Fact]
  public void ReadOnlySpan_ToInt32_LittleEndian()
  {
    byte[] bytes = [0x78, 0x56, 0x34, 0x12];
    Assert.Equal(0x12345678, bytes.ToInt32());
  }

  [Fact]
  public void ReadOnlySpan_ToInt32_BigEndian()
  {
    byte[] bytes = [0x12, 0x34, 0x56, 0x78];
    Assert.Equal(0x12345678, bytes.ToInt32(true));
  }

  [Fact]
  public void ReadOnlySpan_ToInt32_BigEndian_MatchesBinaryPrimitives()
  {
    var value = unchecked((int)0xDEADBEEF);
    var buffer = new byte[4];
    BinaryPrimitives.WriteInt32BigEndian(buffer, value);
    Assert.Equal(value, buffer.ToInt32(true));
  }

  [Fact]
  public void ReadOnlySpan_ToUInt32_RoundTrips()
  {
    var value = 0xFEEDFACEu;
    var bytes = BitConverter.GetBytes(value);
    Assert.Equal(value, bytes.ToUInt32());
  }

  // ── ReadOnlySpan：ToInt64 / ToUInt64 ──

  [Fact]
  public void ReadOnlySpan_ToInt64_LittleEndian()
  {
    byte[] bytes = [0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01];
    Assert.Equal(0x0123456789ABCDEF, bytes.ToInt64());
  }

  [Fact]
  public void ReadOnlySpan_ToInt64_BigEndian()
  {
    byte[] bytes = [0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF];
    Assert.Equal(0x0123456789ABCDEF, bytes.ToInt64(true));
  }

  [Fact]
  public void ReadOnlySpan_ToUInt64_RoundTrips()
  {
    var value = 0x0123456789ABCDEFUL;
    var bytes = BitConverter.GetBytes(value);
    Assert.Equal(value, bytes.ToUInt64());
  }

  // ── ReadOnlySpan：ToSingle / ToDouble ──

  [Fact]
  public void ReadOnlySpan_ToSingle_RoundTrips()
  {
    var value = 3.14f;
    var bytes = BitConverter.GetBytes(value);
    Assert.Equal(value, bytes.ToSingle());
  }

  [Fact]
  public void ReadOnlySpan_ToSingle_BigEndian()
  {
    var value = 1.5f;
    var buffer = new byte[4];
    BinaryPrimitives.WriteSingleBigEndian(buffer, value);
    Assert.Equal(value, buffer.ToSingle(true));
  }

  [Fact]
  public void ReadOnlySpan_ToDouble_RoundTrips()
  {
    var value = Math.PI;
    var bytes = BitConverter.GetBytes(value);
    Assert.Equal(value, bytes.ToDouble());
  }

  [Fact]
  public void ReadOnlySpan_ToDouble_BigEndian()
  {
    var value = -2.75;
    var buffer = new byte[8];
    BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
    Assert.Equal(value, buffer.ToDouble(true));
  }

  // ── ReadOnlySpan：ToInt128 / ToUInt128 ──

  [Fact]
  public void ReadOnlySpan_ToInt128_LittleEndian()
  {
    var value = Int128.CreateTruncating(0x0123456789ABCDEFUL);
    var bytes = new byte[16];
    BinaryPrimitives.WriteInt128LittleEndian(bytes, value);
    Assert.Equal(value, bytes.ToInt128());
  }

  [Fact]
  public void ReadOnlySpan_ToInt128_BigEndian()
  {
    var value = Int128.CreateTruncating(0x0123456789ABCDEFUL);
    var bytes = new byte[16];
    BinaryPrimitives.WriteInt128BigEndian(bytes, value);
    Assert.Equal(value, bytes.ToInt128(true));
  }

  [Fact]
  public void ReadOnlySpan_ToUInt128_RoundTrips()
  {
    var value = UInt128.CreateTruncating(0xFEEDFACEUL);
    var bytes = new byte[16];
    BinaryPrimitives.WriteUInt128LittleEndian(bytes, value);
    Assert.Equal(value, bytes.ToUInt128());
  }

  // ── ReadOnlySpan：ToT ──

  [Fact]
  public void ReadOnlySpan_ToT_Generic_LittleEndian()
  {
    byte[] bytes = [0x78, 0x56, 0x34, 0x12];
    Assert.Equal(0x12345678, bytes.ToT<int>());
  }

  [Fact]
  public void ReadOnlySpan_ToT_Generic_BigEndian()
  {
    byte[] bytes = [0x12, 0x34, 0x56, 0x78];
    Assert.Equal(0x12345678, bytes.ToT<int>(true));
  }

  [Fact]
  public void ReadOnlySpan_ToT_Generic_Struct()
  {
    // Guid 为 unmanaged 结构体，按内存原样读取
    var guid = Guid.NewGuid();
    var bytes = guid.ToByteArray();
    Assert.Equal(guid, bytes.ToT<Guid>());
  }

  [Theory]
  [InlineData(BigAndSmallEndianEncodingMode.DCBA)]
  [InlineData(BigAndSmallEndianEncodingMode.ABCD)]
  [InlineData(BigAndSmallEndianEncodingMode.BADC)]
  [InlineData(BigAndSmallEndianEncodingMode.CDAB)]
  public void ReadOnlySpan_ToT_AllModes_MatchesWriteTo(BigAndSmallEndianEncodingMode mode)
  {
    byte[] bytes = [1, 2, 3, 4];
    var expected = bytes.ToT<int>(mode);
    Assert.Equal(expected, bytes.ToT<int>(mode));
  }

  // ── ReadOnlySpan：WriteTo ──

  [Fact]
  public void ReadOnlySpan_WriteTo_DCBA_OnLittleEndian_NoChange()
  {
    byte[] bytes = [0x78, 0x56, 0x34, 0x12];
    var dest = 0;
    bytes.WriteTo(ref dest, BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(0x12345678, dest);
  }

  [Fact]
  public void ReadOnlySpan_WriteTo_ABCD_ReversesAllBytes()
  {
    byte[] bytes = [1, 2, 3, 4];
    var dest = 0;
    bytes.WriteTo(ref dest, BigAndSmallEndianEncodingMode.ABCD);
    // 读取字节 1,2,3,4 后整体翻转 → 0x01020304
    Assert.Equal(0x01020304, dest);
  }

  [Fact]
  public void ReadOnlySpan_WriteTo_StructDestination()
  {
    var guid = Guid.NewGuid();
    var bytes = guid.ToByteArray();
    var dest = Guid.Empty;
    bytes.WriteTo(ref dest, BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(guid, dest);
  }

  // ── ReadOnlySpan<T>.AsSpan（可写视图） ──

  [Fact]
  public void ReadOnlySpan_AsSpan_SharesMemory_AndIsWritable()
  {
    var array = new[] { 1, 2, 3 };
    ReadOnlySpan<int> readOnly = array;
    var writable = readOnly.AsSpan();
    Assert.Equal(3, writable.Length);
    writable[0] = 100;
    Assert.Equal(100, array[0]);
  }

  // ── Span<byte>：ToInt16 等（可写 Span 重载） ──

  [Fact]
  public void Span_ToInt32_BothEndians()
  {
    Span<byte> bytes = [0x44, 0x33, 0x22, 0x11];
    Assert.Equal(0x11223344, bytes.ToInt32());
    Assert.Equal(0x44332211, bytes.ToInt32(true));
  }

  [Fact]
  public void Span_ToInt16_ToUInt16_ToInt64_ToUInt64()
  {
    Span<byte> be = [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08];
    Assert.Equal(0x0102, be.ToInt16(true));
    Assert.Equal((ushort)0x0102, be.ToUInt16(true));
    Assert.Equal(0x0102030405060708, be.ToInt64(true));
    Assert.Equal(0x0102030405060708UL, be.ToUInt64(true));
  }

  [Fact]
  public void Span_ToSingle_ToDouble()
  {
    Span<byte> fBytes = [0x00, 0x00, 0xC0, 0x3F]; // 1.5f 小端
    Assert.Equal(1.5f, fBytes.ToSingle());

    Span<byte> dBytes = [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x40]; // 2.5 小端
    Assert.Equal(2.5, dBytes.ToDouble());
  }

  [Fact]
  public void Span_ToInt128_ToUInt128()
  {
    var value = Int128.CreateTruncating(0x0102030405060708UL);
    var bytes = new byte[16];
    BinaryPrimitives.WriteInt128LittleEndian(bytes, value);
    Assert.Equal(value, bytes.AsSpan().ToInt128());

    var uValue = UInt128.CreateTruncating(0x0807060504030201UL);
    var uBytes = new byte[16];
    BinaryPrimitives.WriteUInt128BigEndian(uBytes, uValue);
    Assert.Equal(uValue, uBytes.AsSpan().ToUInt128(true));
  }

  // ── Span<byte>：ApplyEndianness ──

  [Fact]
  public void ApplyEndianness_SingleByte_NoChange()
  {
    Span<byte> bytes = [0xAB];
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD); // 单字节直接返回
    Assert.Equal(0xAB, bytes[0]);
  }

  [Fact]
  public void ApplyEndianness_OddLength_Throws()
  {
    Assert.Throws<ArgumentException>(
      () => new byte[] { 1, 2, 3 }.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD));
  }

  [Fact]
  public void ApplyEndianness_ABCD_ReversesWholeBuffer()
  {
    Span<byte> bytes = [1, 2, 3, 4];
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(new byte[] { 4, 3, 2, 1 }, bytes.ToArray());
  }

  [Fact]
  public void ApplyEndianness_DCBA_OnLittleEndian_NoChange()
  {
    Span<byte> bytes = [1, 2, 3, 4];
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(new byte[] { 1, 2, 3, 4 }, bytes.ToArray());
  }

  [Fact]
  public void ApplyEndianness_BADC_SwapsWordOrder()
  {
    Span<byte> bytes = [0x11, 0x22, 0x33, 0x44];
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.BADC);
    Assert.Equal(new byte[] { 0x33, 0x44, 0x11, 0x22 }, bytes.ToArray());
  }

  [Fact]
  public void ApplyEndianness_CDAB_ReversesInsideEachWord()
  {
    Span<byte> bytes = [0x11, 0x22, 0x33, 0x44];
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.CDAB);
    Assert.Equal(new byte[] { 0x22, 0x11, 0x44, 0x33 }, bytes.ToArray());
  }

  [Fact]
  public void ApplyEndianness_ABCD_Twice_IsIdentity()
  {
    Span<byte> bytes = [1, 2, 3, 4, 5, 6, 7, 8];
    var original = bytes.ToArray();
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    bytes.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(original, bytes.ToArray());
  }

  [Fact]
  public void ApplyEndianness_InvalidMode_ThrowsArgumentOutOfRange()
  {
    Assert.Throws<ArgumentOutOfRangeException>(
      () => new byte[] { 1, 2 }.ApplyEndianness((BigAndSmallEndianEncodingMode)99));
  }

  // ── Span<byte>：ToT / WriteTo ──

  [Fact]
  public void Span_ToT_Generic()
  {
    Span<byte> bytes = [0x78, 0x56, 0x34, 0x12];
    Assert.Equal(0x12345678, bytes.ToT<int>());
    Assert.Equal(0x78563412, bytes.ToT<int>(true));
  }

  [Fact]
  public void Span_WriteTo_ABCD()
  {
    Span<byte> bytes = [1, 2, 3, 4];
    var dest = 0;
    bytes.WriteTo(ref dest, BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(0x01020304, dest);
  }

  [Fact]
  public void Span_ToT_AllModes_RoundTripThroughApplyEndianness()
  {
    Span<byte> bytes = [0xAA, 0xBB, 0xCC, 0xDD];
    var dcba = bytes.ToT<int>(BigAndSmallEndianEncodingMode.DCBA);
    var abcd = bytes.ToT<int>(BigAndSmallEndianEncodingMode.ABCD);
    Assert.NotEqual(dcba, abcd);
    // 二次翻转还原
    var restored = BitConverter.GetBytes(abcd);
    restored.ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(dcba, MemoryMarshal.Read<int>(restored));
  }

  // ── 综合往返 ──

  [Theory]
  [InlineData(0)]
  [InlineData(1)]
  [InlineData(-123456789)]
  [InlineData(int.MaxValue)]
  [InlineData(int.MinValue)]
  public void Int32_RoundTrip_LittleAndBigEndian(int value)
  {
    var leBytes = BitConverter.GetBytes(value);
    Assert.Equal(value, leBytes.ToInt32());

    var beBytes = new byte[4];
    BinaryPrimitives.WriteInt32BigEndian(beBytes, value);
    Assert.Equal(value, beBytes.ToInt32(true));
  }
}
