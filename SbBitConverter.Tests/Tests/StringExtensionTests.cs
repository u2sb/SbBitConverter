// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using System.Text;
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

public class StringExtensionTests
{
  [Fact]
  public void IsByte_ValidString() => Assert.True("255".IsByte());

  [Fact]
  public void IsByte_InvalidString()
  {
    Assert.False("256".IsByte());
    Assert.False("abc".IsByte());
  }

  [Fact]
  public void TryParseToByte_Success()
  {
    Assert.True("255".TryParseToByte(out var result));
    Assert.Equal(255, result);
  }

  [Fact]
  public void TryParseToByte_Failure()
  {
    Assert.False("abc".TryParseToByte(out var result));
    Assert.Equal(0, result);
  }

  [Fact]
  public void IsSByte_ValidString()
  {
    Assert.True("127".IsSByte());
    Assert.True("-128".IsSByte());
  }

  [Fact]
  public void IsSByte_InvalidString() => Assert.False("128".IsSByte());

  [Fact]
  public void TryParseToSByte_Success()
  {
    Assert.True("-100".TryParseToSByte(out var result));
    Assert.Equal(-100, result);
  }

  [Fact]
  public void IsInt16_ValidString()
  {
    Assert.True("32767".IsInt16());
    Assert.True("-32768".IsInt16());
  }

  [Fact]
  public void TryParseToInt16_Success()
  {
    Assert.True("12345".TryParseToInt16(out var result));
    Assert.Equal(12345, result);
  }

  [Fact]
  public void IsUInt16_ValidString() => Assert.True("65535".IsUInt16());

  [Fact]
  public void TryParseToUInt16_Success()
  {
    Assert.True("54321".TryParseToUInt16(out var result));
    Assert.Equal(54321, result);
  }

  [Fact]
  public void IsInt32_ValidString()
  {
    Assert.True("2147483647".IsInt32());
    Assert.True("-2147483648".IsInt32());
  }

  [Fact]
  public void TryParseToInt32_Success()
  {
    Assert.True("-42".TryParseToInt32(out var result));
    Assert.Equal(-42, result);
  }

  [Fact]
  public void IsUInt32_ValidString() => Assert.True("4294967295".IsUInt32());

  [Fact]
  public void TryParseToUInt32_Success()
  {
    Assert.True("99999".TryParseToUInt32(out var result));
    Assert.Equal(99999u, result);
  }

  [Fact]
  public void IsInt64_ValidString() => Assert.True("9223372036854775807".IsInt64());

  [Fact]
  public void TryParseToInt64_Success()
  {
    Assert.True("9876543210".TryParseToInt64(out var result));
    Assert.Equal(9876543210L, result);
  }

  [Fact]
  public void IsUInt64_ValidString() => Assert.True("18446744073709551615".IsUInt64());

  [Fact]
  public void TryParseToUInt64_Success()
  {
    Assert.True("123456789012345".TryParseToUInt64(out var result));
    Assert.Equal(123456789012345UL, result);
  }

  [Fact]
  public void IsFloat_ValidString() => Assert.True("3.14".IsFloat());

  [Fact]
  public void TryParseToFloat_Success()
  {
    Assert.True("1.5".TryParseToFloat(out var result));
    Assert.Equal(1.5f, result, 3);
  }

  [Fact]
  public void IsDouble_ValidString() => Assert.True("2.71828".IsDouble());

  [Fact]
  public void TryParseToDouble_Success()
  {
    Assert.True("3.14159".TryParseToDouble(out var result));
    Assert.Equal(3.14159, result, 5);
  }

  [Fact]
  public void EncodingToBytes_ValidString()
  {
    var result = "ABC".EncodingToBytes(Encoding.UTF8);
    Assert.NotNull(result);
    Assert.Equal(3, result.Length);
    Assert.Equal([(byte)'A', (byte)'B', (byte)'C'], result);
  }

  [Fact]
  public void EncodingToBytes_NullString_ReturnsEmpty()
  {
    string? s = null;
    var result = s.EncodingToBytes();
    Assert.Empty(result);
  }

  [Fact]
  public void EncodingToBytes_EmptyString_ReturnsEmpty()
  {
    var result = "".EncodingToBytes();
    Assert.Empty(result);
  }

  [Fact]
  public void IsNullOrEmpty()
  {
    string? s = null;
    Assert.True(s.IsNullOrEmpty());
    Assert.True("".IsNullOrEmpty());
    Assert.False("abc".IsNullOrEmpty());
  }

  [Fact]
  public void IsNullOrWhiteSpace()
  {
    string? s = null;
    Assert.True(s.IsNullOrWhiteSpace());
    Assert.True("  ".IsNullOrWhiteSpace());
    Assert.False(" abc ".IsNullOrWhiteSpace());
  }
}
