using Sb.Extensions.System;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterArray 的序列化：构造(ReadOnlySpan) → ToByteArray → WriteTo
/// </summary>
public class ArraySerializationTests
{
    // ── 构造 ──

    [Fact]
    public void Ctor_FromBytes_Int4_DCBA()
    {
        var bytes = new byte[] { 42, 0, 0, 0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F, 0, 0, 0, 0x80 };
        var v = new Int4(bytes);
        Assert.Equal(42, v[0]);
        Assert.Equal(-1, v[1]);
        Assert.Equal(int.MaxValue, v[2]);
        Assert.Equal(int.MinValue, v[3]);
    }

    [Fact]
    public void Ctor_FromBytes_WithExplicitMode()
    {
        // ABCD 大端：0x01020304 = 16909060
        var bytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
        var v = new Int2(bytes, BigAndSmallEndianEncodingMode.ABCD);
        Assert.Equal(0x01020304, v[0]);
        Assert.Equal(0x05060708, v[1]);
    }

    [Fact]
    public void Ctor_FromBytes_Byte16()
    {
        var bytes = new byte[16];
        for (var i = 0; i < 16; i++) bytes[i] = (byte)(i * 17);

        var v = new Byte16(bytes);
        Assert.Equal(0, v[0]);
        Assert.Equal(17, v[1]);
        Assert.Equal(255, v[15]);
    }

    [Fact]
    public void Ctor_FromBytes_Bool8()
    {
        // DCBA 小端：bool 占 1 字节，非零为 true
        var bytes = new byte[] { 1, 0, 0xFF, 0, 0, 1, 1, 0 };
        var v = new Bool8(bytes);
        Assert.True(v[0]);
        Assert.False(v[1]);
        Assert.True(v[2]);
        Assert.False(v[3]);
        Assert.False(v[4]);
        Assert.True(v[5]);
        Assert.True(v[6]);
        Assert.False(v[7]);
    }

    [Fact]
    public void Ctor_FromBytes_TooShort_Throws()
    {
        var bytes = new byte[8]; // Int4 需要 16 字节
        Assert.Throws<InvalidArrayLengthException>(() => new Int4(bytes));
    }

    // ── ToByteArray ──

    [Fact]
    public void ToByteArray_Default_ReturnsCorrectBytes()
    {
        var v = new Int4();
        v[0] = 42;
        v[1] = -1;
        v[2] = int.MaxValue;
        v[3] = int.MinValue;

        var bytes = v.ToByteArray();
        Assert.Equal(16, bytes.Length);

        // DCBA 小端
        Assert.Equal(42, bytes[0]);
        Assert.Equal(0, bytes[1]);
        Assert.Equal(0, bytes[2]);
        Assert.Equal(0, bytes[3]);
    }

    [Fact]
    public void ToByteArray_WithExplicitMode()
    {
        var v = new Short6(new byte[12], BigAndSmallEndianEncodingMode.BADC);
        var span = v.AsSpan();
        span[0] = 0x0102;
        span[1] = 0x0304;

        var bytes = v.ToByteArray(BigAndSmallEndianEncodingMode.BADC);
        Assert.Equal(12, bytes.Length);
    }

    [Fact]
    public void ToByteArray_RoundTrip_Int4()
    {
        var v = new Int4();
        v[0] = 42;
        v[1] = -1;
        v[2] = int.MaxValue;
        v[3] = int.MinValue;

        var bytes = v.ToByteArray();
        var v2 = new Int4(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
        Assert.Equal(v[3], v2[3]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_Byte16()
    {
        var v = new Byte16();
        var span = v.AsSpan();
        for (byte i = 0; i < 16; i++) span[i] = i;

        var bytes = v.ToByteArray();
        var v2 = new Byte16(bytes);

        for (byte i = 0; i < 16; i++)
            Assert.Equal(i, v2[i]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_Double2()
    {
        var v = new Double2();
        v[0] = 3.14;
        v[1] = -2.718;

        var bytes = v.ToByteArray();
        var v2 = new Double2(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_Float3()
    {
        var v = new Float3();
        v[0] = 1.0f;
        v[1] = 2.0f;
        v[2] = 3.0f;

        var bytes = v.ToByteArray();
        var v2 = new Float3(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_Bool8()
    {
        var v = new Bool8();
        v[0] = true;
        v[1] = false;
        v[2] = true;
        v[3] = false;
        v[4] = true;
        v[5] = false;
        v[6] = true;
        v[7] = false;

        var bytes = v.ToByteArray();
        var v2 = new Bool8(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[7], v2[7]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_LargeArray()
    {
        var v = new Int256();
        var span = v.AsSpan();
        for (var i = 0; i < 256; i++)
            span[i] = i * 100;

        var bytes = v.ToByteArray();
        var v2 = new Int256(bytes);

        for (var i = 0; i < 256; i++)
            Assert.Equal(v[i], v2[i]);
    }

    // ── WriteTo ──

    [Fact]
    public void WriteTo_DefaultMode()
    {
        var v = new Int4();
        v[0] = 100;
        v[1] = 200;

        var dest = new byte[16];
        v.WriteTo(dest);

        var v2 = new Int4(dest);
        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
    }

    [Fact]
    public void WriteTo_TooShort_Throws()
    {
        var v = new Int4();
        var dest = new byte[8]; // 需要 16
        Assert.Throws<InvalidArrayLengthException>(() => v.WriteTo(dest));
    }

    [Fact]
    public void WriteTo_WithExplicitMode()
    {
        var v = new Int4ABCD();
        v[0] = 0x01020304;
        var dest = new byte[16];
        v.WriteTo(dest, BigAndSmallEndianEncodingMode.ABCD);

        Assert.Equal(0x01, dest[0]);
        Assert.Equal(0x02, dest[1]);
        Assert.Equal(0x03, dest[2]);
        Assert.Equal(0x04, dest[3]);
    }
}
