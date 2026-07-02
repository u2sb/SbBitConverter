using Sb.Extensions.System;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterArray 生成的索引器（ref 返回，可通过索引读写）
/// </summary>
public class ArrayIndexerTests
{
    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        var v = new Int4();
        var span = v.AsSpan();
        span[0] = 42;
        span[1] = -1;
        span[2] = int.MaxValue;
        span[3] = int.MinValue;

        Assert.Equal(42, v[0]);
        Assert.Equal(-1, v[1]);
        Assert.Equal(int.MaxValue, v[2]);
        Assert.Equal(int.MinValue, v[3]);
    }

    [Fact]
    public void Indexer_Set_MutatesUnderlyingField()
    {
        var v = new Int4();
        v[0] = 100;
        v[1] = 200;
        v[2] = 300;
        v[3] = 400;

        Assert.Equal(100, v[0]);
        Assert.Equal(200, v[1]);
        Assert.Equal(300, v[2]);
        Assert.Equal(400, v[3]);
    }

    [Fact]
    public void Indexer_LargeArray_AllIndicesWork()
    {
        var v = new Int256();
        var span = v.AsSpan();
        for (var i = 0; i < 256; i++)
            span[i] = i * 10;

        for (var i = 0; i < 256; i++)
            Assert.Equal(i * 10, v[i]);
    }

    [Fact]
    public void Indexer_SingleElement_GetSet()
    {
        var v = new Int1();
        v[0] = 999;
        Assert.Equal(999, v[0]);
    }

    [Fact]
    public void Indexer_ReadOnlyStruct_RefReadonly()
    {
        var v = new UShort4();
        var span = v.AsSpan();
        // ReadOnlySpan 无法写入，但可以验证读取
        Assert.Equal(4, span.Length);
        Assert.Equal((ushort)0, v[0]);
    }

    [Fact]
    public void Indexer_Element128_Works()
    {
        var v = new Int128Arr();
        v[127] = 0x7FFFFFFF;
        Assert.Equal(0x7FFFFFFF, v[127]);
    }

    [Fact]
    public void Indexer_Float_GetSet()
    {
        var v = new Float3();
        v[0] = 1.0f;
        v[1] = 2.0f;
        v[2] = 3.0f;

        Assert.Equal(1.0f, v[0]);
        Assert.Equal(2.0f, v[1]);
        Assert.Equal(3.0f, v[2]);
    }

    [Fact]
    public void Indexer_Double_GetSet()
    {
        var v = new Double2();
        v[0] = 3.14;
        v[1] = -2.718;

        Assert.Equal(3.14, v[0]);
        Assert.Equal(-2.718, v[1]);
    }

    [Fact]
    public void Indexer_Bool_GetSet()
    {
        var v = new Bool8();
        v[0] = true;
        v[1] = false;
        v[7] = true;

        Assert.True(v[0]);
        Assert.False(v[1]);
        Assert.True(v[7]);
    }

    [Fact]
    public void Indexer_ModifyViaRef_VisibleThroughSpan()
    {
        var v = new Int4();
        v[0] = 55;
        v[2] = 77;

        var span = v.AsSpan();
        Assert.Equal(55, span[0]);
        Assert.Equal(77, span[2]);
    }
}
