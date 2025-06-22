using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SbBitConverter.Attributes;

namespace T0;

[SbBitConverterArray(typeof(float), 3, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Float3
{
}

[SbBitConverterArray(typeof(MyStruct), 3, BigAndSmallEndianEncodingMode.ABCD, ElementSize = 8)]
public partial struct MyStructArray3
{
}

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public readonly partial struct MyStruct
{
  [FieldOffset(0)] private readonly Float3 _float3;

  [FieldOffset(4)] private readonly float _f1;
}