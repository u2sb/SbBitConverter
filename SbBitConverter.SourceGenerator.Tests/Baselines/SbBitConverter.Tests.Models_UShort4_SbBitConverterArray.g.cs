// Auto-generated code
#pragma warning disable
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sb.Extensions.System;
using static Sb.Extensions.System.SbBitConverter;
using static Sb.Extensions.System.SpanExtension;
namespace SbBitConverter.Tests.Models
{
[StructLayout(LayoutKind.Explicit, Pack = 2, Size = 8)]
partial struct UShort4
{
  public UShort4(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<UShort4>());
    this._item0 = data.Slice(0, 2).ToT<ushort>(mode);
    this._item1 = data.Slice(2, 2).ToT<ushort>(mode);
    this._item2 = data.Slice(4, 2).ToT<ushort>(mode);
    this._item3 = data.Slice(6, 2).ToT<ushort>(mode);
  }

  [FieldOffset(0)]private readonly ushort _item0;

  [FieldOffset(2)]private readonly ushort _item1;

  [FieldOffset(4)]private readonly ushort _item2;

  [FieldOffset(6)]private readonly ushort _item3;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<UShort4>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<UShort4>());
    this._item0.WriteTo<ushort>(span.Slice(0, 2), mode);
    this._item1.WriteTo<ushort>(span.Slice(2, 2), mode);
    this._item2.WriteTo<ushort>(span.Slice(4, 2), mode);
    this._item3.WriteTo<ushort>(span.Slice(6, 2), mode);
  }

  public int Length => 4;
  public int Count => 4;

  public ref readonly ushort this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ReadOnlySpan<ushort> AsSpan()
  {
    return CreateReadOnlySpan(in _item0, 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ReadOnlySpan<ushort> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateReadOnlySpan(in this[start], length);
  }

}
}
#pragma warning restore
