using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SbBitConverter.Attributes;
using Sb.Extensions.System;

namespace T0.Models;

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 基础类型数组
// ═══════════════════════════════════════════════════════════════

[SbBitConverterArray(typeof(byte), 16)]
public partial struct Byte16 { }

[SbBitConverterArray(typeof(sbyte), 8)]
public partial struct SByte8 { }

[SbBitConverterArray(typeof(bool), 8)]
public partial struct Bool8 { }

[SbBitConverterArray(typeof(short), 6, BigAndSmallEndianEncodingMode.BADC)]
public partial struct Short6 { }

[SbBitConverterArray(typeof(ushort), 4)]
public readonly partial struct UShort4 { }

[SbBitConverterArray(typeof(int), 4)]
public partial struct Int4 { }

[SbBitConverterArray(typeof(uint), 8)]
public partial struct UInt8 { }

[SbBitConverterArray(typeof(long), 4)]
public partial struct Long4 { }

[SbBitConverterArray(typeof(ulong), 4)]
public partial struct ULong4 { }

[SbBitConverterArray(typeof(float), 3, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Float3 { }

[SbBitConverterArray(typeof(double), 2, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Double2 { }

[SbBitConverterArray(typeof(char), 8)]
public partial struct Char8 { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 不同长度
// ═══════════════════════════════════════════════════════════════

[SbBitConverterArray(typeof(int), 1)]
public partial struct Int1 { }

[SbBitConverterArray(typeof(int), 2)]
public partial struct Int2 { }

[SbBitConverterArray(typeof(int), 32)]
public partial struct Int32Arr { }

[SbBitConverterArray(typeof(int), 128)]
public partial struct Int128Arr { }

[SbBitConverterArray(typeof(int), 256)]
public partial struct Int256 { }

[SbBitConverterArray(typeof(byte), 64)]
public partial struct Byte64 { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 全部编码模式
// ═══════════════════════════════════════════════════════════════

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.DCBA)]
public partial struct Int4DCBA { }

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Int4ABCD { }

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.BADC)]
public partial struct Int4BADC { }

[SbBitConverterArray(typeof(int), 4, BigAndSmallEndianEncodingMode.CDAB)]
public partial struct Int4CDAB { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 自定义元素大小
// ═══════════════════════════════════════════════════════════════

[SbBitConverterArray(typeof(int), 8, ElementSize = 4)]
public partial struct Int8CustomSize { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 用户已有 StructLayout
// ═══════════════════════════════════════════════════════════════

[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
[SbBitConverterArray(typeof(int), 4)]
public partial struct Int4WithLayout { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterArray — 大数组 + readonly
// ═══════════════════════════════════════════════════════════════

[SbBitConverterArray(typeof(long), 64)]
public partial struct Long64 { }

[SbBitConverterArray(typeof(double), 64)]
public readonly partial struct Double64 { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterStruct — 基础用法
// ═══════════════════════════════════════════════════════════════

[SbBitConverterStruct(BigAndSmallEndianEncodingMode.DCBA)]
[StructLayout(LayoutKind.Explicit)]
public partial struct MixedStruct
{
    [FieldOffset(0)] public byte B;
    [FieldOffset(1)] public bool Flag;
    [FieldOffset(2)] public short S;
    [FieldOffset(4)] public int I;
}

[SbBitConverterStruct(BigAndSmallEndianEncodingMode.ABCD)]
[StructLayout(LayoutKind.Explicit)]
public partial struct MixedStructABCD
{
    [FieldOffset(0)] public short S1;
    [FieldOffset(2)] public short S2;
    [FieldOffset(4)] public int I;
}

// ═══════════════════════════════════════════════════════════════
// SbBitConverterStruct — 嵌套
// ═══════════════════════════════════════════════════════════════

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public partial struct MyStruct
{
    [FieldOffset(0)] public Float3 Float3;     // 12 bytes, offsets 0-11
    [FieldOffset(12)] public float F1;          // 4 bytes, offset 12-15
}

[SbBitConverterArray(typeof(MyStruct), 3, BigAndSmallEndianEncodingMode.ABCD, ElementSize = 16)]
public readonly partial struct MyStructArray3 { }

// ═══════════════════════════════════════════════════════════════
// SbBitConverterStruct — 属性字段
// ═══════════════════════════════════════════════════════════════

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public partial struct PropertyStruct
{
    [FieldOffset(0)] private int _value;
    [FieldOffset(4)] private float _factor;

    public int Value { get => _value; set => _value = value; }
    public float Factor { get => _factor; set => _factor = value; }
}

// ═══════════════════════════════════════════════════════════════
// SbBitConverterStruct — 同类型多字段
// ═══════════════════════════════════════════════════════════════

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public partial struct MultiFieldStruct
{
    [FieldOffset(0)] public byte B0;
    [FieldOffset(1)] public byte B1;
    [FieldOffset(2)] public byte B2;
    [FieldOffset(3)] public byte B3;
    [FieldOffset(4)] public int I;
}

// ═══════════════════════════════════════════════════════════════
// 枚举类型 — 覆盖 Utils.SizeOfType(TypeKind.Enum) 和 GetEnumSize
// ═══════════════════════════════════════════════════════════════

public enum TestByteEnum : byte { A = 0, B = 1, C = 2 }
public enum TestSByteEnum : sbyte { A = 0, B = 1 }
public enum TestShortEnum : short { A = 0, B = 1 }
public enum TestUShortEnum : ushort { A = 0, B = 1 }
public enum TestIntEnum : int { A = 0, B = 1, C = 2 }
public enum TestUIntEnum : uint { A = 0, B = 1 }
public enum TestLongEnum : long { A = 0, B = 1 }
public enum TestULongEnum : ulong { A = 0, B = 1 }

[SbBitConverterArray(typeof(TestByteEnum), 4)]
public partial struct EnumByte4 { }

[SbBitConverterArray(typeof(TestIntEnum), 4)]
public partial struct EnumInt4 { }

[SbBitConverterArray(typeof(TestLongEnum), 2)]
public partial struct EnumLong2 { }

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public partial struct EnumMixedStruct
{
    [FieldOffset(0)] public TestByteEnum E1;
    [FieldOffset(1)] public TestIntEnum E2;
    [FieldOffset(5)] public TestLongEnum E3;
}

// ═══════════════════════════════════════════════════════════════
// 手工 struct — 测试 unsafe 指针方案可行性
// ═══════════════════════════════════════════════════════════════

[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
public unsafe struct Int4Manual
{
    [FieldOffset(0)] private int _item0;
    [FieldOffset(4)] private int _item1;
    [FieldOffset(8)] private int _item2;
    [FieldOffset(12)] private int _item3;

    public const int Length = 4;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<int> AsSpan()
    {
        return MemoryMarshal.CreateSpan(ref _item0, Length);
    }

    // 方案 P：Unsafe.AsPointer + Unsafe.AsRef（无 fixed 语句）
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref int IndexerP(int index)
    {
        ref var p = ref _item0;
        return ref Unsafe.AsRef<int>((byte*)Unsafe.AsPointer(ref p) + index * sizeof(int));
    }

    // 方案 Q：fixed 语句
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref int IndexerQ(int index)
    {
        fixed (int* p = &_item0)
            return ref p[index];
    }

    // Slice：指针创建 Span
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<int> Slice(int start, int length)
    {
        if ((uint)start > Length || (uint)length > Length - start)
            throw new ArgumentOutOfRangeException();

        fixed (int* p = &_item0)
            return new Span<int>(p + start, length);
    }
}
