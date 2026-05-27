using System.Runtime.InteropServices;
using Sb.Extensions.System;
using SbBitConverter.Attributes;

namespace T0
{
  // 基础数组类型 — float × 3，大端模式
  [SbBitConverterArray(typeof(float), 3, BigAndSmallEndianEncodingMode.ABCD)]
  public partial struct Float3
  {
  }

  // struct 数组
  [SbBitConverterArray(typeof(MyStruct), 3, BigAndSmallEndianEncodingMode.ABCD, ElementSize = 8)]
  public readonly partial struct MyStructArray3
  {
  }

  [SbBitConverterStruct]
  [StructLayout(LayoutKind.Explicit)]
  public partial struct MyStruct
  {
    [FieldOffset(0)] private Float3 _float3;

    [FieldOffset(4)] private float _f1;
  }

  // ── 新增测试类型 ──

  // bool 数组 — 测试 System_Boolean 支持
  [SbBitConverterArray(typeof(bool), 8)]
  public partial struct Bool8
  {
  }

  // int 数组 — DCBA（小端）模式
  [SbBitConverterArray(typeof(int), 4)]
  public partial struct Int4
  {
  }

  // short 数组 — BADC 编码测试
  [SbBitConverterArray(typeof(short), 6, BigAndSmallEndianEncodingMode.BADC)]
  public partial struct Short6
  {
  }

  // double 数组 — 大端模式
  [SbBitConverterArray(typeof(double), 2, BigAndSmallEndianEncodingMode.ABCD)]
  public partial struct Double2
  {
  }

  // byte 数组 — 测试大批量
  [SbBitConverterArray(typeof(byte), 16)]
  public partial struct Byte16
  {
  }

  // 混合 struct — 测试 SbBitConverterStruct 生成器
  [SbBitConverterStruct(BigAndSmallEndianEncodingMode.DCBA)]
  [StructLayout(LayoutKind.Explicit)]
  public partial struct MixedStruct
  {
    [FieldOffset(0)] public byte B;

    [FieldOffset(1)] public bool Flag;

    [FieldOffset(2)] public short S;

    [FieldOffset(4)] public int I;
  }

  // readonly 数组 — 测试 readonly struct 支持
  [SbBitConverterArray(typeof(ushort), 4)]
  public readonly partial struct UShort4
  {
  }
}
