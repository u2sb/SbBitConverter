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
partial struct Bool8
{
  public Bool8(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Bool8>());
    this._item0 = data[0] != 0;
    this._item1 = data[1] != 0;
    this._item2 = data[2] != 0;
    this._item3 = data[3] != 0;
    this._item4 = data[4] != 0;
    this._item5 = data[5] != 0;
    this._item6 = data[6] != 0;
    this._item7 = data[7] != 0;
  }

  [FieldOffset(0)]private bool _item0;

  [FieldOffset(1)]private bool _item1;

  [FieldOffset(2)]private bool _item2;

  [FieldOffset(3)]private bool _item3;

  [FieldOffset(4)]private bool _item4;

  [FieldOffset(5)]private bool _item5;

  [FieldOffset(6)]private bool _item6;

  [FieldOffset(7)]private bool _item7;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Bool8>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Bool8>());
    MemoryMarshal.AsBytes(AsSpan()).CopyTo(span);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Bool8>());
    this._item0 = data[0] != 0;
    this._item1 = data[1] != 0;
    this._item2 = data[2] != 0;
    this._item3 = data[3] != 0;
    this._item4 = data[4] != 0;
    this._item5 = data[5] != 0;
    this._item6 = data[6] != 0;
    this._item7 = data[7] != 0;
  }

  public int Length => 8;
  public int Count => 8;

  public ref bool this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<bool> AsSpan()
  {
    return CreateSpan(ref _item0, 8);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<bool> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
