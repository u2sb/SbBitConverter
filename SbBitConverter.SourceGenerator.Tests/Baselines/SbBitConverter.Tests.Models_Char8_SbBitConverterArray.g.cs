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
[StructLayout(LayoutKind.Explicit, Pack = 2, Size = 16)]
partial struct Char8
{
  public Char8(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Char8>());
    this._item0 = data.Slice(0, 2).ToT<char>(mode);
    this._item1 = data.Slice(2, 2).ToT<char>(mode);
    this._item2 = data.Slice(4, 2).ToT<char>(mode);
    this._item3 = data.Slice(6, 2).ToT<char>(mode);
    this._item4 = data.Slice(8, 2).ToT<char>(mode);
    this._item5 = data.Slice(10, 2).ToT<char>(mode);
    this._item6 = data.Slice(12, 2).ToT<char>(mode);
    this._item7 = data.Slice(14, 2).ToT<char>(mode);
  }

  [FieldOffset(0)]private char _item0;

  [FieldOffset(2)]private char _item1;

  [FieldOffset(4)]private char _item2;

  [FieldOffset(6)]private char _item3;

  [FieldOffset(8)]private char _item4;

  [FieldOffset(10)]private char _item5;

  [FieldOffset(12)]private char _item6;

  [FieldOffset(14)]private char _item7;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Char8>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Char8>());
    this._item0.WriteTo<char>(span.Slice(0, 2), mode);
    this._item1.WriteTo<char>(span.Slice(2, 2), mode);
    this._item2.WriteTo<char>(span.Slice(4, 2), mode);
    this._item3.WriteTo<char>(span.Slice(6, 2), mode);
    this._item4.WriteTo<char>(span.Slice(8, 2), mode);
    this._item5.WriteTo<char>(span.Slice(10, 2), mode);
    this._item6.WriteTo<char>(span.Slice(12, 2), mode);
    this._item7.WriteTo<char>(span.Slice(14, 2), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Char8>());
    this._item0.ReadFromBytes(data.Slice(0, 2), mode);
    this._item1.ReadFromBytes(data.Slice(2, 2), mode);
    this._item2.ReadFromBytes(data.Slice(4, 2), mode);
    this._item3.ReadFromBytes(data.Slice(6, 2), mode);
    this._item4.ReadFromBytes(data.Slice(8, 2), mode);
    this._item5.ReadFromBytes(data.Slice(10, 2), mode);
    this._item6.ReadFromBytes(data.Slice(12, 2), mode);
    this._item7.ReadFromBytes(data.Slice(14, 2), mode);
  }

  public int Length => 8;
  public int Count => 8;

  public ref char this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<char> AsSpan()
  {
    return CreateSpan(ref _item0, 8);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<char> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
