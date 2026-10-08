// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
partial struct PropertyStruct
{
  public PropertyStruct(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<PropertyStruct>());
    this._value = data.Slice(0, 4).ToT<int>(mode);
    this._factor = data.Slice(4, 4).ToT<float>(mode);

  }

  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<PropertyStruct>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<PropertyStruct>());
    this._value.WriteTo<int>(span.Slice(0, 4), mode);
    this._factor.WriteTo<float>(span.Slice(4, 4), mode);

  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<PropertyStruct>());
    this._value = data.Slice(0, 4).ToT<int>(mode);
    this._factor = data.Slice(4, 4).ToT<float>(mode);

  }

}
}
#pragma warning restore
