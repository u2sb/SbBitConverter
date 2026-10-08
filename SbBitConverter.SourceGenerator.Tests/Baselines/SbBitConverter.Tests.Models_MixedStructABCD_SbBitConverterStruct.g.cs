// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct MixedStructABCD
{
  public MixedStructABCD(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<MixedStructABCD>());
    this.S1 = data.Slice(0, 2).ToT<short>(mode);
    this.S2 = data.Slice(2, 2).ToT<short>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    var data = new byte[Unsafe.SizeOf<MixedStructABCD>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(span, Unsafe.SizeOf<MixedStructABCD>());
    this.S1.WriteTo<short>(span.Slice(0, 2), mode);
    this.S2.WriteTo<short>(span.Slice(2, 2), mode);
    this.I.WriteTo<int>(span.Slice(4, 4), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<MixedStructABCD>());
    this.S1 = data.Slice(0, 2).ToT<short>(mode);
    this.S2 = data.Slice(2, 2).ToT<short>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

}
}
#pragma warning restore
