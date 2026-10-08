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
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
partial struct SByte8
{
  public SByte8(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<SByte8>());
    this._item0 = unchecked((sbyte)data[0]);
    this._item1 = unchecked((sbyte)data[1]);
    this._item2 = unchecked((sbyte)data[2]);
    this._item3 = unchecked((sbyte)data[3]);
    this._item4 = unchecked((sbyte)data[4]);
    this._item5 = unchecked((sbyte)data[5]);
    this._item6 = unchecked((sbyte)data[6]);
    this._item7 = unchecked((sbyte)data[7]);
  }

  [FieldOffset(0)]private sbyte _item0;

  [FieldOffset(1)]private sbyte _item1;

  [FieldOffset(2)]private sbyte _item2;

  [FieldOffset(3)]private sbyte _item3;

  [FieldOffset(4)]private sbyte _item4;

  [FieldOffset(5)]private sbyte _item5;

  [FieldOffset(6)]private sbyte _item6;

  [FieldOffset(7)]private sbyte _item7;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<SByte8>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<SByte8>());
    MemoryMarshal.AsBytes(AsSpan()).CopyTo(span);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<SByte8>());
    this._item0 = unchecked((sbyte)data[0]);
    this._item1 = unchecked((sbyte)data[1]);
    this._item2 = unchecked((sbyte)data[2]);
    this._item3 = unchecked((sbyte)data[3]);
    this._item4 = unchecked((sbyte)data[4]);
    this._item5 = unchecked((sbyte)data[5]);
    this._item6 = unchecked((sbyte)data[6]);
    this._item7 = unchecked((sbyte)data[7]);
  }

  public int Length => 8;
  public int Count => 8;

  public ref sbyte this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<sbyte> AsSpan()
  {
    return CreateSpan(ref _item0, 8);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<sbyte> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
