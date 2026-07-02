using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterArray 生成的 Length / Count / AsSpan / Slice
/// </summary>
public class ArrayPropertiesTests
{
    [Fact]
    public void Length_MatchesAttributeValue()
    {
        Assert.Equal(4, new Int4().Length);
        Assert.Equal(8, new Bool8().Length);
        Assert.Equal(6, new Short6().Length);
        Assert.Equal(3, new Float3().Length);
        Assert.Equal(2, new Double2().Length);
        Assert.Equal(16, new Byte16().Length);
        Assert.Equal(1, new Int1().Length);
        Assert.Equal(256, new Int256().Length);
    }

    [Fact]
    public void Count_Equals_Length()
    {
        var v = new Int4();
        Assert.Equal(v.Length, v.Count);

        var b = new Byte16();
        Assert.Equal(b.Length, b.Count);
    }

    [Fact]
    public void AsSpan_ReadableAndWritable()
    {
        var v = new Int4();
        var span = v.AsSpan();

        Assert.Equal(4, span.Length);

        span[0] = 10;
        span[1] = 20;
        span[2] = 30;
        span[3] = 40;

        Assert.Equal(10, span[0]);
        Assert.Equal(20, span[1]);
        Assert.Equal(30, span[2]);
        Assert.Equal(40, span[3]);
    }

    [Fact]
    public void AsSpan_ReadOnlyStruct_ReturnsReadOnlySpan()
    {
        var v = new UShort4();
        // 编译期验证：AsSpan() 返回 ReadOnlySpan<ushort>
        var roSpan = v.AsSpan();
        Assert.Equal(4, roSpan.Length);
    }

    [Fact]
    public void Slice_Valid_ReturnsCorrectSubSpan()
    {
        var v = new Int4();
        var span = v.AsSpan();
        span[0] = 100;
        span[1] = 200;
        span[2] = 300;
        span[3] = 400;

        var slice = v.Slice(1, 2);
        Assert.Equal(2, slice.Length);
        Assert.Equal(200, slice[0]);
        Assert.Equal(300, slice[1]);
    }

    [Fact]
    public void Slice_StartZero_FullCopy()
    {
        var v = new Byte16();
        var span = v.AsSpan();
        for (byte i = 0; i < 16; i++)
            span[i] = i;

        var slice = v.Slice(0, 16);
        for (byte i = 0; i < 16; i++)
            Assert.Equal(i, slice[i]);
    }

    [Fact]
    public void Slice_NegativeStart_Throws()
    {
        var v = new Int4();
        Assert.Throws<ArgumentOutOfRangeException>(() => v.Slice(-1, 2));
    }

    [Fact]
    public void Slice_NegativeLength_Throws()
    {
        var v = new Int4();
        Assert.Throws<ArgumentOutOfRangeException>(() => v.Slice(0, -1));
    }

    [Fact]
    public void Slice_OutOfRange_Throws()
    {
        var v = new Int4();
        Assert.Throws<ArgumentOutOfRangeException>(() => v.Slice(2, 3));
    }

    [Fact]
    public void AsSpan_LargeArray_Works()
    {
        var v = new Int256();
        var span = v.AsSpan();
        Assert.Equal(256, span.Length);

        for (var i = 0; i < 256; i++)
            span[i] = i;

        Assert.Equal(0, span[0]);
        Assert.Equal(255, span[255]);
    }
}
