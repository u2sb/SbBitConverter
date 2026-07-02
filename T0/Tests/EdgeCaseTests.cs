using System.Runtime.CompilerServices;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试极端和边界情况
/// </summary>
public class EdgeCaseTests
{
    // ── 零初始化 ──

    [Fact]
    public void DefaultConstructor_AllFieldsZero()
    {
        var v = new Int4();
        Assert.Equal(0, v[0]);
        Assert.Equal(0, v[1]);
        Assert.Equal(0, v[2]);
        Assert.Equal(0, v[3]);
    }

    [Fact]
    public void DefaultConstructor_Byte16_AllZero()
    {
        var v = new Byte16();
        for (var i = 0; i < 16; i++)
            Assert.Equal((byte)0, v[i]);
    }

    [Fact]
    public void DefaultConstructor_Float3_AllZero()
    {
        var v = new Float3();
        Assert.Equal(0f, v[0]);
        Assert.Equal(0f, v[1]);
        Assert.Equal(0f, v[2]);
    }

    // ── 极值 ──

    [Fact]
    public void Int_MinMax_Preserved()
    {
        var v = new Int4();
        v[0] = int.MinValue;
        v[1] = int.MaxValue;
        v[2] = 0;
        v[3] = -1;

        var bytes = v.ToByteArray();
        var v2 = new Int4(bytes);

        Assert.Equal(int.MinValue, v2[0]);
        Assert.Equal(int.MaxValue, v2[1]);
        Assert.Equal(0, v2[2]);
        Assert.Equal(-1, v2[3]);
    }

    [Fact]
    public void UInt_MaxValue_Preserved()
    {
        var v = new UInt8();
        v[0] = uint.MaxValue;
        v[7] = 0xDEADBEEF;

        var bytes = v.ToByteArray();
        var v2 = new UInt8(bytes);

        Assert.Equal(uint.MaxValue, v2[0]);
        Assert.Equal(0xDEADBEEFu, v2[7]);
    }

    [Fact]
    public void ULong_RoundTrip()
    {
        var v = new ULong4();
        v[0] = ulong.MinValue;
        v[1] = ulong.MaxValue;
        v[2] = 0xDEADBEEFCAFEBABE;
        v[3] = 42;

        var bytes = v.ToByteArray();
        var v2 = new ULong4(bytes);

        Assert.Equal(ulong.MinValue, v2[0]);
        Assert.Equal(ulong.MaxValue, v2[1]);
        Assert.Equal(0xDEADBEEFCAFEBABEul, v2[2]);
        Assert.Equal(42ul, v2[3]);
    }

    [Fact]
    public void Long_MinMax_Preserved()
    {
        var v = new Long4();
        v[0] = long.MinValue;
        v[1] = long.MaxValue;
        v[2] = -1;
        v[3] = 0x1234567890ABCDEF;

        var bytes = v.ToByteArray();
        var v2 = new Long4(bytes);

        Assert.Equal(long.MinValue, v2[0]);
        Assert.Equal(long.MaxValue, v2[1]);
        Assert.Equal(-1, v2[2]);
        Assert.Equal(0x1234567890ABCDEF, v2[3]);
    }

    [Fact]
    public void Float_SpecialValues_Preserved()
    {
        var v = new Float3();
        v[0] = float.NaN;
        v[1] = float.PositiveInfinity;
        v[2] = float.NegativeInfinity;

        var bytes = v.ToByteArray();
        var v2 = new Float3(bytes);

        Assert.True(float.IsNaN(v2[0]));
        Assert.True(float.IsPositiveInfinity(v2[1]));
        Assert.True(float.IsNegativeInfinity(v2[2]));
    }

    // ── 空数据构造 ──

    [Fact]
    public void Ctor_FromEmptyBytes_ThenDefault_Works()
    {
        var bytes = new byte[Unsafe.SizeOf<Int4>()];
        var v = new Int4(bytes);
        Assert.Equal(0, v[0]);
    }

    // ── SByte / Char 类型 ──

    [Fact]
    public void SByte_RoundTrip()
    {
        var bytes = new byte[]
        {
            0x7F, 0x80, 0xFF, 0x00, 0x01, 0xFE, 0x7E, 0x00
        };
        var v = new SByte8(bytes);
        Assert.Equal(127, v[0]);
        Assert.Equal(-128, v[1]);
        Assert.Equal(-1, v[2]);
        Assert.Equal(0, v[3]);
    }

    [Fact]
    public void Char_RoundTrip()
    {
        var v = new Char8();
        v[0] = 'A';
        v[1] = '中';
        v[7] = 'Z';

        var bytes = v.ToByteArray();
        var v2 = new Char8(bytes);

        Assert.Equal('A', v2[0]);
        Assert.Equal('中', v2[1]);
        Assert.Equal('Z', v2[7]);
    }

    // ── 修改后 Span 一致性 ──

    [Fact]
    public void Span_Modifications_ReflectedInIndexer()
    {
        var v = new Int4();
        var span = v.AsSpan();

        span[0] = 1;
        span[1] = 2;
        span[2] = 4;
        span[3] = 8;

        Assert.Equal(1, v[0]);
        Assert.Equal(2, v[1]);
        Assert.Equal(4, v[2]);
        Assert.Equal(8, v[3]);
    }

    [Fact]
    public void Indexer_Modifications_ReflectedInSpan()
    {
        var v = new Int4();
        v[0] = 10;
        v[1] = 20;
        v[2] = 30;
        v[3] = 40;

        var span = v.AsSpan();
        Assert.Equal(10, span[0]);
        Assert.Equal(20, span[1]);
        Assert.Equal(30, span[2]);
        Assert.Equal(40, span[3]);
    }

    // ── Slice 边界 ──

    [Fact]
    public void Slice_StartZero_LengthZero()
    {
        var v = new Int4();
        var slice = v.Slice(0, 0);
        Assert.Equal(0, slice.Length);
    }

    [Fact]
    public void Slice_StartInBounds_LengthZero_Works()
    {
        var v = new Int4();
        v[0] = 42;
        // start=0, length=0 — 不访问任何元素，不会触发索引越界
        var slice = v.Slice(0, 0);
        Assert.Equal(0, slice.Length);
    }

    // ── 大数组 ──

    [Fact]
    public void Long64_RoundTrip()
    {
        var v = new Long64();
        var span = v.AsSpan();
        span[0] = long.MinValue;
        span[63] = long.MaxValue;
        span[32] = -1;

        var bytes = v.ToByteArray();
        var v2 = new Long64(bytes);

        Assert.Equal(long.MinValue, v2[0]);
        Assert.Equal(long.MaxValue, v2[63]);
        Assert.Equal(-1, v2[32]);
    }

    [Fact]
    public void Double64_ReadOnlyStruct_Works()
    {
        var v = new Double64();
        // Double64 为 readonly struct，AsSpan() 返回 ReadOnlySpan<double>
        var roSpan = v.AsSpan();
        Assert.Equal(64, roSpan.Length);
        Assert.Equal(64, v.Length);

        // 通过字节构造验证
        var bytes = new byte[Unsafe.SizeOf<Double64>()];
        var v2 = new Double64(bytes);
        Assert.Equal(64, v2.Length);
    }

    [Fact]
    public void Byte64_RoundTrip()
    {
        var v = new Byte64();
        var span = v.AsSpan();
        for (byte i = 0; i < 64; i++)
            span[i] = (byte)(i * 4 + 1);

        var bytes = v.ToByteArray();
        var v2 = new Byte64(bytes);

        Assert.Equal(1, v2[0]);
        Assert.Equal(129, v2[32]);
        Assert.Equal(253, v2[63]);
    }
}
