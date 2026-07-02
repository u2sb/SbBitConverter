using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterStruct 的生成代码
/// </summary>
public class StructGeneratorTests
{
    // ── 构造 ──

    [Fact]
    public void Ctor_FromBytes_DeserializesCorrectly()
    {
        // DCBA 小端：byte=0xAB, bool=01, short=FC18(= -1000 补码), int=00BC614E(=12345678)
        var bytes = new byte[]
        {
            0xAB,       // offset 0: byte
            0x01,       // offset 1: bool (true)
            0x18, 0xFC, // offset 2-3: short -1000 (小端)
            0x4E, 0x61, 0xBC, 0x00 // offset 4-7: int 12345678 (小端)
        };
        var v = new MixedStruct(bytes);

        Assert.Equal(0xAB, v.B);
        Assert.True(v.Flag);
        Assert.Equal(-1000, v.S);
        Assert.Equal(12345678, v.I);
    }

    [Fact]
    public void Ctor_FromBytes_WithExplicitMode()
    {
        // ABCD 大端
        var bytes = new byte[]
        {
            0x00, 0x64, // S1 = 100 (大端)
            0xFF, 0x9C, // S2 = -100 (大端)
            0x00, 0x01, 0x00, 0x00 // I = 65536 (大端)
        };
        var v = new MixedStructABCD(bytes);
        Assert.Equal(100, v.S1);
        Assert.Equal(-100, v.S2);
        Assert.Equal(65536, v.I);
    }

    [Fact]
    public void Ctor_FromBytes_TooShort_Throws()
    {
        var bytes = new byte[4]; // MixedStruct 需要 8
        Assert.Throws<InvalidArrayLengthException>(() => new MixedStruct(bytes));
    }

    // ── ToByteArray ──

    [Fact]
    public void ToByteArray_DefaultMode()
    {
        var v = new MixedStruct
        {
            B = 0xAB,
            Flag = true,
            S = -1000,
            I = 12345678
        };
        var bytes = v.ToByteArray();
        Assert.Equal(8, bytes.Length);

        Assert.Equal(0xAB, bytes[0]);
        Assert.Equal(1, bytes[1]);
    }

    [Fact]
    public void ToByteArray_RoundTrip_PreservesValues()
    {
        var v = new MixedStruct
        {
            B = 0xCD,
            Flag = false,
            S = 2000,
            I = -987654321
        };
        var bytes = v.ToByteArray();
        var v2 = new MixedStruct(bytes);

        Assert.Equal(v.B, v2.B);
        Assert.Equal(v.Flag, v2.Flag);
        Assert.Equal(v.S, v2.S);
        Assert.Equal(v.I, v2.I);
    }

    [Fact]
    public void ToByteArray_ABCD_RoundTrip()
    {
        var v = new MixedStructABCD
        {
            S1 = 30000,
            S2 = -30000,
            I = 0x12345678
        };
        var bytes = v.ToByteArray();
        var v2 = new MixedStructABCD(bytes);

        Assert.Equal(v.S1, v2.S1);
        Assert.Equal(v.S2, v2.S2);
        Assert.Equal(v.I, v2.I);
    }

    // ── WriteTo ──

    [Fact]
    public void WriteTo_DefaultMode_Succeeds()
    {
        var v = new MixedStruct
        {
            B = 0xFF,
            Flag = true,
            S = 1,
            I = 2
        };
        var dest = new byte[16];
        v.WriteTo(dest);

        var v2 = new MixedStruct(dest);
        Assert.Equal(v.B, v2.B);
        Assert.Equal(v.Flag, v2.Flag);
        Assert.Equal(v.S, v2.S);
        Assert.Equal(v.I, v2.I);
    }

    [Fact]
    public void WriteTo_TooShort_Throws()
    {
        var v = new MixedStruct();
        var dest = new byte[4];
        Assert.Throws<InvalidArrayLengthException>(() => v.WriteTo(dest));
    }

    // ── 属性字段 ──

    [Fact]
    public void PropertyFields_SerializeAndDeserialize()
    {
        var v = new PropertyStruct { Value = 42, Factor = 3.14f };
        var bytes = v.ToByteArray();
        var v2 = new PropertyStruct(bytes);

        Assert.Equal(v.Value, v2.Value);
        Assert.Equal(v.Factor, v2.Factor);
    }

    [Fact]
    public void PropertyFields_CanBeSetAndRead()
    {
        var v = new PropertyStruct();
        v.Value = 999;
        v.Factor = 2.718f;

        Assert.Equal(999, v.Value);
        Assert.Equal(2.718f, v.Factor);
    }

    // ── 同类型多字段 ──

    [Fact]
    public void MultiField_RoundTrip()
    {
        var v = new MultiFieldStruct
        {
            B0 = 0x01,
            B1 = 0x02,
            B2 = 0x03,
            B3 = 0x04,
            I = 0x12345678
        };
        var bytes = v.ToByteArray();
        var v2 = new MultiFieldStruct(bytes);

        Assert.Equal(v.B0, v2.B0);
        Assert.Equal(v.B1, v2.B1);
        Assert.Equal(v.B2, v2.B2);
        Assert.Equal(v.B3, v2.B3);
        Assert.Equal(v.I, v2.I);
    }

    [Fact]
    public void MultiField_BytesCorrectOrder()
    {
        var v = new MultiFieldStruct
        {
            B0 = 0xAA,
            B1 = 0xBB,
            B2 = 0xCC,
            B3 = 0xDD,
            I = 0
        };
        var bytes = v.ToByteArray();
        Assert.Equal(8, bytes.Length);
        Assert.Equal(0xAA, bytes[0]);
        Assert.Equal(0xBB, bytes[1]);
        Assert.Equal(0xCC, bytes[2]);
        Assert.Equal(0xDD, bytes[3]);
    }

    // ── 嵌套结构体 ──

    [Fact]
    public void NestedStruct_SizeOf()
    {
        // MyStruct: Float3(12 bytes) at offset 0 + float(4 bytes) at offset 12 = 16 bytes
        Assert.Equal(16, Unsafe.SizeOf<MyStruct>());
    }

    [Fact]
    public void NestedStructArray_RoundTrip()
    {
        // MyStructArray3: 3 × MyStruct(16 bytes each) = 48 bytes
        var bytes = new byte[48];
        var v = new MyStructArray3(bytes);
        var span = v.AsSpan();

        // 验证可以反序列化
        Assert.Equal(3, v.Length);
        Assert.Equal(3, span.Length);
    }

    // ── 不同编码模式对比 ──

    [Fact]
    public void DifferentModes_ProduceDifferentBytes()
    {
        var v1 = new MixedStruct { B = 0x12, Flag = true, S = 0x3456, I = 0x789ABCDE };
        var bytesDCBA = v1.ToByteArray(BigAndSmallEndianEncodingMode.DCBA);
        var bytesABCD = v1.ToByteArray(BigAndSmallEndianEncodingMode.ABCD);

        // B, Flag 是 1 字节，不变
        Assert.Equal(bytesDCBA[0], bytesABCD[0]);
        Assert.Equal(bytesDCBA[1], bytesABCD[1]);

        // short, int 顺序不同
        Assert.NotEqual(bytesDCBA[2], bytesABCD[2]);
    }
}
