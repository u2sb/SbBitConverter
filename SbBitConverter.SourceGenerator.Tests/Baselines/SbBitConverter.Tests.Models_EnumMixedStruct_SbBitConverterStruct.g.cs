// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct EnumMixedStruct
{
  public EnumMixedStruct(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumMixedStruct>());
    this.E1 = data.Slice(0, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
    this.E2 = data.Slice(1, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
    this.E3 = data.Slice(5, 8).ToT<SbBitConverter.Tests.Models.TestLongEnum>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<EnumMixedStruct>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<EnumMixedStruct>());
    this.E1.WriteTo<SbBitConverter.Tests.Models.TestByteEnum>(span.Slice(0, 1), mode);
    this.E2.WriteTo<SbBitConverter.Tests.Models.TestIntEnum>(span.Slice(1, 4), mode);
    this.E3.WriteTo<SbBitConverter.Tests.Models.TestLongEnum>(span.Slice(5, 8), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumMixedStruct>());
    this.E1 = data.Slice(0, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
    this.E2 = data.Slice(1, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
    this.E3 = data.Slice(5, 8).ToT<SbBitConverter.Tests.Models.TestLongEnum>(mode);

  }

}
}
#pragma warning restore
