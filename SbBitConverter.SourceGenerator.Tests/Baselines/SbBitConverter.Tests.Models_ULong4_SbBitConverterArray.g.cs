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
[StructLayout(LayoutKind.Explicit, Pack = 8, Size = 32)]
partial struct ULong4
{
  public ULong4(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<ULong4>());
    this._item0 = data.Slice(0, 8).ToT<ulong>(mode);
    this._item1 = data.Slice(8, 8).ToT<ulong>(mode);
    this._item2 = data.Slice(16, 8).ToT<ulong>(mode);
    this._item3 = data.Slice(24, 8).ToT<ulong>(mode);
  }

  [FieldOffset(0)]private ulong _item0;

  [FieldOffset(8)]private ulong _item1;

  [FieldOffset(16)]private ulong _item2;

  [FieldOffset(24)]private ulong _item3;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<ULong4>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<ULong4>());
    this._item0.WriteTo<ulong>(span.Slice(0, 8), mode);
    this._item1.WriteTo<ulong>(span.Slice(8, 8), mode);
    this._item2.WriteTo<ulong>(span.Slice(16, 8), mode);
    this._item3.WriteTo<ulong>(span.Slice(24, 8), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<ULong4>());
    this._item0.ReadFromBytes(data.Slice(0, 8), mode);
    this._item1.ReadFromBytes(data.Slice(8, 8), mode);
    this._item2.ReadFromBytes(data.Slice(16, 8), mode);
    this._item3.ReadFromBytes(data.Slice(24, 8), mode);
  }

  public int Length => 4;
  public int Count => 4;

  public ref ulong this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<ulong> AsSpan()
  {
    return CreateSpan(ref _item0, 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<ulong> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
