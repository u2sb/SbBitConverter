using System.Runtime.CompilerServices;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试枚举类型在 SbBitConverterArray 和 SbBitConverterStruct 中的使用
///   覆盖 Utils.SizeOfType(TypeKind.Enum) 和 GetEnumSize
/// </summary>
public class EnumTypeTests
{
    // ── 枚举数组 ──

    [Fact]
    public void EnumByte4_SizeOf()
    {
        // byte-based enum = 1 byte each, 4 elements
        Assert.Equal(4, Unsafe.SizeOf<EnumByte4>());
    }

    [Fact]
    public void EnumByte4_RoundTrip()
    {
        var v = new EnumByte4();
        v[0] = TestByteEnum.C;
        v[1] = TestByteEnum.A;
        v[2] = TestByteEnum.B;
        v[3] = TestByteEnum.A;

        var bytes = v.ToByteArray();
        var v2 = new EnumByte4(bytes);

        Assert.Equal(TestByteEnum.C, v2[0]);
        Assert.Equal(TestByteEnum.A, v2[1]);
        Assert.Equal(TestByteEnum.B, v2[2]);
        Assert.Equal(TestByteEnum.A, v2[3]);
    }

    [Fact]
    public void EnumInt4_SizeOf()
    {
        // int-based enum = 4 bytes each, 4 elements
        Assert.Equal(16, Unsafe.SizeOf<EnumInt4>());
    }

    [Fact]
    public void EnumInt4_RoundTrip()
    {
        var v = new EnumInt4();
        v[0] = TestIntEnum.A;
        v[1] = TestIntEnum.B;
        v[2] = TestIntEnum.C;
        v[3] = TestIntEnum.A;

        var bytes = v.ToByteArray();
        var v2 = new EnumInt4(bytes);

        Assert.Equal(TestIntEnum.A, v2[0]);
        Assert.Equal(TestIntEnum.B, v2[1]);
        Assert.Equal(TestIntEnum.C, v2[2]);
        Assert.Equal(TestIntEnum.A, v2[3]);
    }

    [Fact]
    public void EnumLong2_SizeOf()
    {
        // long-based enum = 8 bytes each, 2 elements
        Assert.Equal(16, Unsafe.SizeOf<EnumLong2>());
    }

    [Fact]
    public void EnumLong2_RoundTrip()
    {
        var v = new EnumLong2();
        v[0] = TestLongEnum.A;
        v[1] = TestLongEnum.B;

        var bytes = v.ToByteArray();
        var v2 = new EnumLong2(bytes);

        Assert.Equal(TestLongEnum.A, v2[0]);
        Assert.Equal(TestLongEnum.B, v2[1]);
    }

    // ── 枚举字段在 Struct 中 ──

    [Fact]
    public void EnumMixedStruct_SizeOf()
    {
        // E1(byte enum, 1 byte) at offset 0
        // E2(int enum, 4 bytes) at offset 1
        // E3(long enum, 8 bytes) at offset 5
        // Total = 13 bytes, but runtime pads to alignment
        var size = Unsafe.SizeOf<EnumMixedStruct>();
        Assert.True(size >= 13);
    }

    [Fact]
    public void EnumMixedStruct_RoundTrip()
    {
        var v = new EnumMixedStruct
        {
            E1 = TestByteEnum.C,
            E2 = TestIntEnum.B,
            E3 = TestLongEnum.A
        };
        var bytes = v.ToByteArray();
        var v2 = new EnumMixedStruct(bytes);

        Assert.Equal(v.E1, v2.E1);
        Assert.Equal(v.E2, v2.E2);
        Assert.Equal(v.E3, v2.E3);
    }

    // ── 其他枚举底层类型 ──

    [Fact]
    public void Enum_UnderlyingTypes()
    {
        // 验证不同底层类型的枚举均可作为数组元素
        Assert.Equal(1, Unsafe.SizeOf<TestSByteEnum>());
        Assert.Equal(2, Unsafe.SizeOf<TestShortEnum>());
        Assert.Equal(2, Unsafe.SizeOf<TestUShortEnum>());
        Assert.Equal(4, Unsafe.SizeOf<TestUIntEnum>());
        Assert.Equal(8, Unsafe.SizeOf<TestULongEnum>());
    }
}
