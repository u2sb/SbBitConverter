// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   测试 SbBitConverter 的基本转换方法
/// </summary>
public class BestBitConverterTests
{
  // ── ToByteArray ──

  [Fact]
  public void ToByteArray_Int32_DCBA()
  {
    var value = 0x12345678;
    var bytes = value.ToByteArray(BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(4, bytes.Length);
    Assert.Equal(0x78, bytes[0]);
    Assert.Equal(0x56, bytes[1]);
    Assert.Equal(0x34, bytes[2]);
    Assert.Equal(0x12, bytes[3]);
  }

  [Fact]
  public void ToByteArray_Int32_ABCD()
  {
    var value = 0x12345678;
    var bytes = value.ToByteArray(BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(0x12, bytes[0]);
    Assert.Equal(0x34, bytes[1]);
    Assert.Equal(0x56, bytes[2]);
    Assert.Equal(0x78, bytes[3]);
  }

  [Fact]
  public void ToByteArray_Default_RespectsSystemEndianness()
  {
    var value = 42;
    var bytes = value.ToByteArray();
    if (BitConverter.IsLittleEndian)
    {
      // DCBA on little-endian system
      Assert.Equal(42, bytes[0]);
    }
  }

  [Fact]
  public void ToByteArray_Byte()
  {
    var value = (byte)0xAB;
    var bytes = value.ToByteArray();
    Assert.Equal(1, bytes.Length);
    Assert.Equal(0xAB, bytes[0]);
  }

  [Fact]
  public void ToByteArray_Int16()
  {
    var value = (short)0x1234;
    var bytes = value.ToByteArray(BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(2, bytes.Length);
  }

  [Fact]
  public void ToByteArray_Int64()
  {
    var value = 0x1234567890ABCDEFL;
    var bytes = value.ToByteArray();
    Assert.Equal(8, bytes.Length);
  }

  [Fact]
  public void ToByteArray_Float()
  {
    var value = 1.0f;
    var bytes = value.ToByteArray();
    Assert.Equal(4, bytes.Length);
  }

  [Fact]
  public void ToByteArray_Double()
  {
    var value = 3.14159;
    var bytes = value.ToByteArray();
    Assert.Equal(8, bytes.Length);
  }

  // ── ReadFromBytes ──

  [Fact]
  public void ReadFromBytes_Int32_DCBA()
  {
    var bytes = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    var value = 0;
    value.ReadFromBytes(bytes, BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(0x12345678, value);
  }

  [Fact]
  public void ReadFromBytes_Int32_ABCD()
  {
    var bytes = new byte[] { 0x12, 0x34, 0x56, 0x78 };
    var value = 0;
    value.ReadFromBytes(bytes, BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(0x12345678, value);
  }

  [Fact]
  public void ReadFromBytes_TooShort_Throws()
  {
    var bytes = new byte[2];
    var value = 0;
    Assert.Throws<InvalidArrayLengthException>(() => value.ReadFromBytes(bytes, BigAndSmallEndianEncodingMode.DCBA));
  }

  // ── WriteTo ──

  [Fact]
  public void WriteTo_Int32_DCBA()
  {
    var value = 0x12345678;
    var dest = new byte[4];
    value.WriteTo(dest, BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(0x78, dest[0]);
    Assert.Equal(0x56, dest[1]);
    Assert.Equal(0x34, dest[2]);
    Assert.Equal(0x12, dest[3]);
  }

  [Fact]
  public void WriteTo_Int32_ABCD()
  {
    var value = 0x12345678;
    var dest = new byte[4];
    value.WriteTo(dest, BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(0x12, dest[0]);
    Assert.Equal(0x34, dest[1]);
    Assert.Equal(0x56, dest[2]);
    Assert.Equal(0x78, dest[3]);
  }

  [Fact]
  public void WriteTo_TooShort_Throws()
  {
    var value = 42;
    var dest = new byte[2];
    Assert.Throws<ArgumentException>(() => value.WriteTo(dest, BigAndSmallEndianEncodingMode.DCBA));
  }

  // ── RoundTrip ──

  [Fact]
  public void RoundTrip_Int32()
  {
    var original = -987654321;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<int>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_UInt32()
  {
    var original = 0xDEADBEEFu;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<uint>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_Float()
  {
    var original = 3.14159f;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<float>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_Double()
  {
    var original = -2.718281828;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<double>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_Byte()
  {
    var original = (byte)0xC3;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<byte>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_Short()
  {
    var original = (short)-10000;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<short>();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void RoundTrip_Long()
  {
    var original = 0x0123456789ABCDEFL;
    var bytes = original.ToByteArray();
    var restored = bytes.ToT<long>();
    Assert.Equal(original, restored);
  }

  // ── Span.ToT ──

  [Fact]
  public void Span_ToT_Int32()
  {
    ReadOnlySpan<byte> span = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    var value = span.ToT<int>(BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(0x12345678, value);
  }

  [Fact]
  public void Span_ToT_WithBigEndianBoolean()
  {
    ReadOnlySpan<byte> span = new byte[] { 0x12, 0x34, 0x56, 0x78 };
    var value = span.ToT<int>(true);
    Assert.Equal(0x12345678, value);
  }

  [Fact]
  public void Span_ToInt16()
  {
    ReadOnlySpan<byte> span = new byte[] { 0x34, 0x12 };
    var value = span.ToInt16();
    Assert.Equal(0x1234, value);
  }

  [Fact]
  public void Span_ToInt16_BigEndian()
  {
    ReadOnlySpan<byte> span = new byte[] { 0x12, 0x34 };
    var value = span.ToInt16(true);
    Assert.Equal(0x1234, value);
  }

  [Fact]
  public void Span_ToInt32()
  {
    ReadOnlySpan<byte> span = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    var value = span.ToInt32();
    Assert.Equal(0x12345678, value);
  }

  [Fact]
  public void Span_ToInt64()
  {
    ReadOnlySpan<byte> span = new byte[] { 0xEF, 0xCD, 0xAB, 0x89, 0x67, 0x45, 0x23, 0x01 };
    var value = span.ToInt64();
    Assert.Equal(0x0123456789ABCDEFL, value);
  }

  [Fact]
  public void Span_ToUInt16()
  {
    ReadOnlySpan<byte> span = new byte[] { 0xFF, 0xFE };
    var value = span.ToUInt16();
    Assert.Equal(0xFEFFu, value);
  }

  [Fact]
  public void Span_ToUInt32()
  {
    ReadOnlySpan<byte> span = new byte[] { 0xEF, 0xBE, 0xAD, 0xDE };
    var value = span.ToUInt32();
    Assert.Equal(0xDEADBEEF, value);
  }

  [Fact]
  public void Span_ToUInt64()
  {
    ReadOnlySpan<byte> span = new byte[] { 0xBE, 0xBA, 0xFE, 0xCA, 0xEF, 0xBE, 0xAD, 0xDE };
    var value = span.ToUInt64();
    Assert.Equal(0xDEADBEEFCAFEBABEul, value);
  }

  [Fact]
  public void Span_ToSingle()
  {
    var original = 3.14159f;
    var bytes = BitConverter.GetBytes(original);
    ReadOnlySpan<byte> span = bytes;
    var restored = span.ToSingle();
    Assert.Equal(original, restored);
  }

  [Fact]
  public void Span_ToDouble()
  {
    var original = 2.718281828;
    var bytes = BitConverter.GetBytes(original);
    ReadOnlySpan<byte> span = bytes;
    var restored = span.ToDouble();
    Assert.Equal(original, restored);
  }

  // ── Span.WriteTo ──

  [Fact]
  public void Span_WriteTo_Int32()
  {
    var bytes = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    ReadOnlySpan<byte> span = bytes;
    var dest = 0;
    span.WriteTo(ref dest, BigAndSmallEndianEncodingMode.DCBA);
    Assert.Equal(0x12345678, dest);
  }

  // ── CheckLength ──

  [Fact]
  public void CheckLength_Valid_NoException()
  {
    var data = new byte[16];
    Sb.Extensions.System.SbBitConverter.CheckLength(data, 16);
  }

  [Fact]
  public void CheckLength_Valid_GreaterThanExpected()
  {
    var data = new byte[32];
    Sb.Extensions.System.SbBitConverter.CheckLength(data, 16);
  }

  [Fact]
  public void CheckLength_TooShort_Throws()
  {
    var data = new byte[8];
    Assert.Throws<InvalidArrayLengthException>(() =>
      Sb.Extensions.System.SbBitConverter.CheckLength(data, 16));
  }

  [Fact]
  public void CheckLength_Span_TooShort_Throws()
  {
    var data = new byte[8];
    Assert.Throws<InvalidArrayLengthException>(() =>
      Sb.Extensions.System.SbBitConverter.CheckLength(data.AsSpan(), 16));
  }

  [Fact]
  public void CheckLength_ReadOnlySpan_TooShort_Throws()
  {
    var data = new byte[8];
    Assert.Throws<InvalidArrayLengthException>(() =>
      Sb.Extensions.System.SbBitConverter.CheckLength((ReadOnlySpan<byte>)data, 16));
  }

  // ── AsByteSpan / AsReadOnlyByteSpan ──

  [Fact]
  public void AsByteSpan_Int32_ReturnsFourBytes()
  {
    var value = 0x12345678;
    var span = value.AsByteSpan();
    Assert.Equal(4, span.Length);
  }

  [Fact]
  public void AsReadOnlyByteSpan_Int32_ReturnsFourBytes()
  {
    var value = 0x12345678;
    var span = value.AsReadOnlyByteSpan();
    Assert.Equal(4, span.Length);
  }

  [Fact]
  public void AsByteSpan_Modify_ReflectedInValue()
  {
    var value = 0;
    var span = value.AsByteSpan();
    span[0] = 0x78;
    span[1] = 0x56;
    span[2] = 0x34;
    span[3] = 0x12;
    Assert.Equal(0x12345678, value);
  }
}
