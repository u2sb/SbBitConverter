using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SbBitConverter.Attributes;

namespace T0;

#if UNSAFE
#endif

[SbBitConverterArray(typeof(float), 3, BigAndSmallEndianEncodingMode.ABCD)]
public partial struct Float3
{
}

[SbBitConverterStruct]
[StructLayout(LayoutKind.Explicit)]
public partial struct MyStruct
{
  [FieldOffset(0)] private Float3 _float3;

  [FieldOffset(4)] private float _f1;
}