// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct MultiFieldStruct
{
  public MultiFieldStruct(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MultiFieldStruct>());
    this.B0 = data.Slice(0, 1).ToT<byte>(mode);
    this.B1 = data.Slice(1, 1).ToT<byte>(mode);
    this.B2 = data.Slice(2, 1).ToT<byte>(mode);
    this.B3 = data.Slice(3, 1).ToT<byte>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<MultiFieldStruct>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<MultiFieldStruct>());
    this.B0.WriteTo<byte>(span.Slice(0, 1), mode);
    this.B1.WriteTo<byte>(span.Slice(1, 1), mode);
    this.B2.WriteTo<byte>(span.Slice(2, 1), mode);
    this.B3.WriteTo<byte>(span.Slice(3, 1), mode);
    this.I.WriteTo<int>(span.Slice(4, 4), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<MultiFieldStruct>());
    this.B0 = data.Slice(0, 1).ToT<byte>(mode);
    this.B1 = data.Slice(1, 1).ToT<byte>(mode);
    this.B2 = data.Slice(2, 1).ToT<byte>(mode);
    this.B3 = data.Slice(3, 1).ToT<byte>(mode);
    this.I = data.Slice(4, 4).ToT<int>(mode);

  }

}
}
#pragma warning restore
