// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct MixedStruct
{
  public MixedStruct(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MixedStruct>());
    this.B = data.Slice(0, 1).ToT<byte>(mode);
    this.Flag = data.Slice(1, 1).ToT<bool>(mode);
    this.S = data.Slice(2, 2).ToT<short>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<MixedStruct>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<MixedStruct>());
    this.B.WriteTo<byte>(span.Slice(0, 1), mode);
    this.Flag.WriteTo<bool>(span.Slice(1, 1), mode);
    this.S.WriteTo<short>(span.Slice(2, 2), mode);
    this.I.WriteTo<int>(span.Slice(4, 4), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MixedStruct>());
    this.B = data.Slice(0, 1).ToT<byte>(mode);
    this.Flag = data.Slice(1, 1).ToT<bool>(mode);
    this.S = data.Slice(2, 2).ToT<short>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

}
}
#pragma warning restore
