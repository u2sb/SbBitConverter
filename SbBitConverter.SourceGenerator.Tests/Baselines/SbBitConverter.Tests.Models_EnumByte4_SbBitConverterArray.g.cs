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
[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 4)]
partial struct EnumByte4
{
  public EnumByte4(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumByte4>());
    this._item0 = data.Slice(0, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
    this._item1 = data.Slice(1, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
    this._item2 = data.Slice(2, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
    this._item3 = data.Slice(3, 1).ToT<SbBitConverter.Tests.Models.TestByteEnum>(mode);
  }

  [FieldOffset(0)]private SbBitConverter.Tests.Models.TestByteEnum _item0;

  [FieldOffset(1)]private SbBitConverter.Tests.Models.TestByteEnum _item1;

  [FieldOffset(2)]private SbBitConverter.Tests.Models.TestByteEnum _item2;

  [FieldOffset(3)]private SbBitConverter.Tests.Models.TestByteEnum _item3;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<EnumByte4>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<EnumByte4>());
    this._item0.WriteTo<SbBitConverter.Tests.Models.TestByteEnum>(span.Slice(0, 1), mode);
    this._item1.WriteTo<SbBitConverter.Tests.Models.TestByteEnum>(span.Slice(1, 1), mode);
    this._item2.WriteTo<SbBitConverter.Tests.Models.TestByteEnum>(span.Slice(2, 1), mode);
    this._item3.WriteTo<SbBitConverter.Tests.Models.TestByteEnum>(span.Slice(3, 1), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumByte4>());
    this._item0.ReadFromBytes(data.Slice(0, 1), mode);
    this._item1.ReadFromBytes(data.Slice(1, 1), mode);
    this._item2.ReadFromBytes(data.Slice(2, 1), mode);
    this._item3.ReadFromBytes(data.Slice(3, 1), mode);
  }

  public int Length => 4;
  public int Count => 4;

  public ref SbBitConverter.Tests.Models.TestByteEnum this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<SbBitConverter.Tests.Models.TestByteEnum> AsSpan()
  {
    return CreateSpan(ref _item0, 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<SbBitConverter.Tests.Models.TestByteEnum> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
