// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   测试 SbBitConverter 的大小端应用
/// </summary>
public class EndiannessTests
{
  // ── WithEndianness ──

  [Fact]
  public void WithEndianness_Byte_NoChange()
  {
    var value = (byte)0xA5;
    var result = value.WithEndianness(true);
    Assert.Equal(0xA5, result);
  }

  [Fact]
  public void WithEndianness_Int16_SwapsOnBigEndianMode()
  {
    var value = (short)0x1234;
    var bigEndianResult = value.WithEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      // 在 little-endian 系统上，使用 big-endian mode 会翻转
      Assert.Equal(0x3412, bigEndianResult);
    }
  }

  [Fact]
  public void WithEndianness_Int16_NoChangeOnLittleEndianMode()
  {
    var value = (short)0x1234;
    var result = value.WithEndianness(false);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x1234, result);
    }
  }

  [Fact]
  public void WithEndianness_Int32_SwapsOnBigEndianMode()
  {
    var value = 0x12345678;
    var result = value.WithEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x78563412, result);
    }
  }

  [Fact]
  public void WithEndianness_Int64_SwapsOnBigEndianMode()
  {
    var value = 0x0123456789ABCDEFL;
    var result = value.WithEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(unchecked((long)0xEFCDAB8967452301), result);
    }
  }

  // ── ApplyEndianness (ref) ──

  [Fact]
  public void ApplyEndianness_Int32_ModifiesInPlace()
  {
    var value = 0x12345678;
    value.ApplyEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x78563412, value);
    }
  }

  [Fact]
  public void WithEndianness_SByte_NoChange()
  {
    var value = (sbyte)-128;
    var result = value.WithEndianness(true);
    Assert.Equal(-128, result);
  }

  [Fact]
  public void ApplyEndianness_Int16()
  {
    var value = (short)0x0102;
    value.ApplyEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x0201, value);
    }
  }

  [Fact]
  public void ApplyEndianness_Int64()
  {
    var value = 0x0123456789ABCDEFL;
    value.ApplyEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(unchecked((long)0xEFCDAB8967452301), value);
    }
  }

  [Fact]
  public void ApplyEndianness_UInt32()
  {
    var value = 0x12345678u;
    value.ApplyEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x78563412u, value);
    }
  }

  [Fact]
  public void ApplyEndianness_UInt16()
  {
    var value = (ushort)0x1234;
    value.ApplyEndianness(true);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x3412, value);
    }
  }

  // ── Span.ApplyEndianness ──

  [Fact]
  public void Span_ApplyEndianness_DCBA_NoChangeOnLE()
  {
    var data = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    var span = data.AsSpan();
    span.ApplyEndianness(BigAndSmallEndianEncodingMode.DCBA);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x78, data[0]);
    }
  }

  [Fact]
  public void Span_ApplyEndianness_ABCD_ReversesOnLE()
  {
    var data = new byte[] { 0x78, 0x56, 0x34, 0x12 };
    data.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x12, data[0]);
    }
  }

  [Fact]
  public void Span_ApplyEndianness_SingleByte_NoChange()
  {
    var data = new byte[] { 0xAB };
    data.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    Assert.Equal(0xAB, data[0]);
  }

  [Fact]
  public void Span_ApplyEndianness_OddLength_Throws()
  {
    var data = new byte[] { 0x01, 0x02, 0x03 };
    Assert.Throws<ArgumentException>(() => data.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD));
  }

  [Fact]
  public void Span_ApplyEndianness_AllModes_RoundTrip()
  {
    var original = new byte[] { 0x01, 0x02, 0x03, 0x04 };

    // DCBA on LE = no-op
    var data1 = original.ToArray();
    data1.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.DCBA);

    // ABCD on LE = reverse
    var data2 = original.ToArray();
    data2.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);

    // 两者应当不等
    if (BitConverter.IsLittleEndian)
    {
      Assert.NotEqual(data1[0], data2[0]);
    }
  }

  // ── Span.Reverse 边界 ──

  [Fact]
  public void Span_ApplyEndianness_TwoBytes()
  {
    var data = new byte[] { 0x01, 0x02 };
    data.AsSpan().ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);

    if (BitConverter.IsLittleEndian)
    {
      Assert.Equal(0x02, data[0]);
      Assert.Equal(0x01, data[1]);
    }
  }
}
