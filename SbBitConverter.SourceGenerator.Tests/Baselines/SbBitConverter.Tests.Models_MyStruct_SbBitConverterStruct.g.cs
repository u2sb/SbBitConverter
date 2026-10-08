// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct MyStruct
{
  public MyStruct(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MyStruct>());
    this.Float3 = default; this.Float3.ReadFromBytes(data.Slice(0, Unsafe.SizeOf<SbBitConverter.Tests.Models.Float3>()), mode);
    this.F1 = data.Slice(12, 4).ToT<float>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<MyStruct>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<MyStruct>());
    this.Float3.WriteTo(span.Slice(0, Unsafe.SizeOf<SbBitConverter.Tests.Models.Float3>()), mode);
    this.F1.WriteTo<float>(span.Slice(12, 4), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MyStruct>());
    this.Float3.ReadFromBytes(data.Slice(0, Unsafe.SizeOf<SbBitConverter.Tests.Models.Float3>()), mode);
    this.F1 = data.Slice(12, 4).ToT<float>(mode);

  }

}
}
#pragma warning restore
