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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 8)]
partial struct Int2
{
  public Int2(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int2>());
    this._item0 = data.Slice(0, 4).ToT<int>(mode);
    this._item1 = data.Slice(4, 4).ToT<int>(mode);
  }

  [FieldOffset(0)]private int _item0;

  [FieldOffset(4)]private int _item1;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<Int2>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<Int2>());
    this._item0.WriteTo<int>(span.Slice(0, 4), mode);
    this._item1.WriteTo<int>(span.Slice(4, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<Int2>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
  }

  public int Length => 2;
  public int Count => 2;

  public ref int this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<int> AsSpan()
  {
    return CreateSpan(ref _item0, 2);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<int> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
