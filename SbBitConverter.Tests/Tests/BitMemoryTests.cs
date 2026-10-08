// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

public class BitMemoryTests
{
  [Fact]
  public void Constructor_Default_LengthIs8PerByte()
  {
    var mem = new BitMemory(new byte[4]);
    Assert.Equal(32, mem.Length);
  }

  [Fact]
  public void Constructor_ByteArray_PropertiesValid()
  {
    var bytes = new byte[2] { 0xAA, 0xBB };
    var mem = new BitMemory(bytes);
    Assert.Equal(16, mem.Length);
    Assert.True(mem.Memory.Span.SequenceEqual(bytes));
  }

  [Fact]
  public void Constructor_WithBitCountAndOffset()
  {
    var bytes = new byte[4];
    var mem = new BitMemory(bytes, 20, 4);
    Assert.Equal(20, mem.Length);
  }

  [Fact]
  public void Constructor_NegativeBitCount_Throws() =>
    Assert.Throws<ArgumentOutOfRangeException>(() => new BitMemory(new byte[1], -1));

  [Fact]
  public void Constructor_StartBitOffsetOutOfRange_Throws()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() => new BitMemory(new byte[4], 8, 8));
    Assert.Throws<ArgumentOutOfRangeException>(() => new BitMemory(new byte[4], 8, -1));
  }

  [Fact]
  public void Constructor_InsufficientBuffer_Throws() =>
    Assert.Throws<ArgumentException>(() => new BitMemory(new byte[1], 9));

  [Fact]
  public void Constructor_UshortMemory()
  {
    var mem = new BitMemory(new ushort[4].AsMemory());
    Assert.Equal(64, mem.Length);
  }

  [Fact]
  public void Slice_Valid()
  {
    var mem = new BitMemory(new byte[8]);
    var slice = mem.Slice(8, 32);
    Assert.Equal(32, slice.Length);
  }

  [Fact]
  public void Slice_Invalid_Throws()
  {
    var mem = new BitMemory(new byte[8]);
    Assert.ThrowsAny<Exception>(() => mem.Slice(70, 1));
  }

  [Fact]
  public void Equals_SameBitMemory()
  {
    var bytes = new byte[4];
    var a = new BitMemory(bytes, 32);
    var b = new BitMemory(bytes, 32);
    Assert.True(a.Equals(b));
    Assert.True(a == b);
  }

  [Fact]
  public void Equals_DifferentBitMemory()
  {
    var a = new BitMemory(new byte[2], 16);
    var b = new BitMemory(new byte[3], 24);
    Assert.False(a.Equals(b));
    Assert.True(a != b);
  }

  [Fact]
  public void BitSpan_ReturnsValidBitSpan()
  {
    var mem = new BitMemory(new byte[1]);
    var span = mem.BitSpan;
    Assert.True(span.Length > 0);
  }
}
