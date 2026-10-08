// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

public class BitSpanTests
{
  [Fact]
  public void Constructor_FromByteSpan()
  {
    var bytes = new byte[4] { 0xFF, 0x00, 0xAA, 0x55 };
    var span = new BitSpan(bytes.AsSpan());
    Assert.Equal(32, span.Length);
  }

  [Fact]
  public void Constructor_WithBitCount()
  {
    var bytes = new byte[2];
    var span = new BitSpan(bytes.AsSpan(), 10);
    Assert.Equal(10, span.Length);
  }

  [Fact]
  public void Set_And_Get()
  {
    var bytes = new byte[1];
    var span = new BitSpan(bytes.AsSpan());
    span.Set(0, true);
    Assert.True(span.Get(0));
    Assert.False(span.Get(1));
    span.Set(7, true);
    Assert.True(span.Get(7));
    Assert.Equal(0x81, bytes[0]);
  }

  [Fact]
  public void Set_False_ClearsBit()
  {
    var bytes = new byte[1] { 0xFF };
    var span = new BitSpan(bytes.AsSpan());
    span.Set(3, false);
    Assert.False(span.Get(3));
  }

  [Fact]
  public void Indexer_GetSet()
  {
    var bytes = new byte[1];
    var span = new BitSpan(bytes.AsSpan());
    span[2] = true;
    Assert.True(span[2]);
    span[2] = false;
    Assert.False(span[2]);
  }

  [Fact]
  public void SetAll()
  {
    var bytes = new byte[4];
    var span = new BitSpan(bytes.AsSpan());
    span.SetAll(true);
    for (var i = 0; i < 32; i++)
    {
      Assert.True(span.Get(i));
    }
  }

  [Fact]
  public void Not()
  {
    var bytes = new byte[2] { 0xAA, 0x55 };
    var span = new BitSpan(bytes.AsSpan());
    span.Not();
    Assert.Equal(0x55, bytes[0]);
    Assert.Equal(0xAA, bytes[1]);
  }

  [Fact]
  public void And()
  {
    var bytes1 = new byte[1] { 0x0F };
    var bytes2 = new byte[1] { 0xF0 };
    var span1 = new BitSpan(bytes1.AsSpan());
    var span2 = new BitSpan(bytes2.AsSpan());
    span1.And(span2);
    Assert.Equal(0, bytes1[0]);
  }

  [Fact]
  public void Or()
  {
    var bytes1 = new byte[1] { 0x0F };
    var bytes2 = new byte[1] { 0xF0 };
    var span1 = new BitSpan(bytes1.AsSpan());
    var span2 = new BitSpan(bytes2.AsSpan());
    span1.Or(span2);
    Assert.Equal(0xFF, bytes1[0]);
  }

  [Fact]
  public void Xor()
  {
    var bytes1 = new byte[1] { 0xFF };
    var bytes2 = new byte[1] { 0x0F };
    var span1 = new BitSpan(bytes1.AsSpan());
    var span2 = new BitSpan(bytes2.AsSpan());
    span1.Xor(span2);
    Assert.Equal(0xF0, bytes1[0]);
  }

  [Fact]
  public void CopyTo_ToBoolSpan()
  {
    var bytes = new byte[2] { 0xFF, 0xFF };
    var span = new BitSpan(bytes.AsSpan());
    var dest = new bool[16];
    span.CopyTo(dest.AsSpan());
    Assert.All(dest, Assert.True);
  }

  [Fact]
  public void CopyTo_ToBytes()
  {
    var bytes = new byte[2];
    var span = new BitSpan(bytes.AsSpan());
    span.Set(3, true);
    span.Set(10, true);
    var dest = new byte[2];
    span.CopyTo(dest.AsSpan(), 0, 16);
    Assert.Equal(bytes[0], dest[0]);
    Assert.Equal(bytes[1], dest[1]);
  }

  [Fact]
  public void Slice_Valid()
  {
    var bytes = new byte[4];
    var span = new BitSpan(bytes.AsSpan());
    var slice = span.Slice(8, 16);
    Assert.Equal(16, slice.Length);
    slice.Set(0, true);
    Assert.True(span.Get(8));
  }

  [Fact]
  public void ReadOnlyBitSpan_FromReadOnlyBytes()
  {
    var bytes = new byte[2] { 0x01, 0x00 };
    var roSpan = new ReadOnlyBitSpan(bytes.AsSpan());
    Assert.True(roSpan.Get(0));
    Assert.False(roSpan.Get(1));
  }

  [Fact]
  public void ReadOnlyBitSpan_Indexer()
  {
    var bytes = new byte[1] { 0x02 };
    var roSpan = new ReadOnlyBitSpan(bytes.AsSpan());
    Assert.False(roSpan[0]);
    Assert.True(roSpan[1]);
  }

  [Fact]
  public void GetByte()
  {
    var bytes = new byte[1] { 0xAA };
    var span = new BitSpan(bytes.AsSpan());
    var val = span.GetByte(0);
    Assert.Equal(0xAA, val);
  }

  [Fact]
  public void ToBoolArray()
  {
    var bytes = new byte[1] { 0x03 };
    var span = new BitSpan(bytes.AsSpan());
    var arr = span.ToBoolArray();
    Assert.True(arr[0]);
    Assert.True(arr[1]);
    Assert.False(arr[2]);
  }

  [Fact]
  public void And_LengthMismatch_Throws()
  {
    var b1 = new byte[1];
    var b2 = new byte[2];
    var s1 = new BitSpan(b1.AsSpan());
    var s2 = new BitSpan(b2.AsSpan());
    // ref struct 不能被 lambda 捕获，只能显式 try/catch
    try
    {
      s1.And(s2);
      Assert.Fail("Expected ArgumentException");
    }
    catch (ArgumentException)
    {
    }
  }
}
