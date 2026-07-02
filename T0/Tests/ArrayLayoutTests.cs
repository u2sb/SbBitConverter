using System.Runtime.CompilerServices;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterArray 生成的 struct 内存布局（FieldOffset / StructLayout）
/// </summary>
public class ArrayLayoutTests
{
    [Fact]
    public void SizeOf_Matches_Length_Times_ElementSize()
    {
        Assert.Equal(16, Unsafe.SizeOf<Int4>());          // 4 × 4
        Assert.Equal(8, Unsafe.SizeOf<Bool8>());          // 8 × 1
        Assert.Equal(12, Unsafe.SizeOf<Float3>());        // 3 × 4
        Assert.Equal(16, Unsafe.SizeOf<Double2>());       // 2 × 8
        Assert.Equal(16, Unsafe.SizeOf<Byte16>());        // 16 × 1
        Assert.Equal(4, Unsafe.SizeOf<Int1>());           // 1 × 4
        Assert.Equal(128, Unsafe.SizeOf<Int32Arr>());     // 32 × 4
        Assert.Equal(512, Unsafe.SizeOf<Int128Arr>());    // 128 × 4
        Assert.Equal(1024, Unsafe.SizeOf<Int256>());      // 256 × 4
        Assert.Equal(6 * 2, Unsafe.SizeOf<Short6>());     // 6 × 2
    }

    [Fact]
    public void Layout_SequentialFields_Contiguous()
    {
        var v = new Int4();
        v[0] = 0x41;
        v[1] = 0x42;
        v[2] = 0x43;
        v[3] = 0x44;

        var bytes = v.ToByteArray();
        // DCBA 小端
        Assert.Equal(0x41, bytes[0]);
        Assert.Equal(0x42, bytes[4]);
        Assert.Equal(0x43, bytes[8]);
        Assert.Equal(0x44, bytes[12]);
    }

    [Fact]
    public void Layout_CustomStructLayout_Preserved()
    {
        // 验证 SizeOf 使用自定义的 Pack/Size
        Assert.Equal(16, Unsafe.SizeOf<Int4WithLayout>());
    }

    [Fact]
    public void Layout_ReadOnlyStruct_FieldsAreReadOnly()
    {
        var v = new UShort4();
        Assert.Equal(4, v.Length);

        // AsSpan() 返回 ReadOnlySpan<ushort>
        var roSpan = v.AsSpan();
        Assert.Equal(4, roSpan.Length);
    }

    [Fact]
    public void Layout_CustomElementSize_Respected()
    {
        var v = new Int8CustomSize();
        Assert.Equal(8, v.Length);
        Assert.Equal(32, Unsafe.SizeOf<Int8CustomSize>()); // 8 × 4
    }

    [Fact]
    public void Layout_ModifySpan_ReflectedInStruct()
    {
        var v = new Int4();
        var span = v.AsSpan();
        span[0] = unchecked((int)0xDEADBEEF);

        Assert.Equal(unchecked((int)0xDEADBEEF), v[0]);

        // 通过原始指针验证
        span[2] = int.MaxValue;
        Assert.Equal(int.MaxValue, v[2]);
    }
}
