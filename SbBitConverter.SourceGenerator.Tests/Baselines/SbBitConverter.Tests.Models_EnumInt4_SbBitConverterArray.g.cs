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
[StructLayout(LayoutKind.Explicit, Pack = 4, Size = 16)]
partial struct EnumInt4
{
  public EnumInt4(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumInt4>());
    this._item0 = data.Slice(0, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
    this._item1 = data.Slice(4, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
    this._item2 = data.Slice(8, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
    this._item3 = data.Slice(12, 4).ToT<SbBitConverter.Tests.Models.TestIntEnum>(mode);
  }

  [FieldOffset(0)]private SbBitConverter.Tests.Models.TestIntEnum _item0;

  [FieldOffset(4)]private SbBitConverter.Tests.Models.TestIntEnum _item1;

  [FieldOffset(8)]private SbBitConverter.Tests.Models.TestIntEnum _item2;

  [FieldOffset(12)]private SbBitConverter.Tests.Models.TestIntEnum _item3;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    var data = new byte[Unsafe.SizeOf<EnumInt4>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(span, Unsafe.SizeOf<EnumInt4>());
    this._item0.WriteTo<SbBitConverter.Tests.Models.TestIntEnum>(span.Slice(0, 4), mode);
    this._item1.WriteTo<SbBitConverter.Tests.Models.TestIntEnum>(span.Slice(4, 4), mode);
    this._item2.WriteTo<SbBitConverter.Tests.Models.TestIntEnum>(span.Slice(8, 4), mode);
    this._item3.WriteTo<SbBitConverter.Tests.Models.TestIntEnum>(span.Slice(12, 4), mode);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void ReadFromBytes(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)0)
  {
    CheckLength(data, Unsafe.SizeOf<EnumInt4>());
    this._item0.ReadFromBytes(data.Slice(0, 4), mode);
    this._item1.ReadFromBytes(data.Slice(4, 4), mode);
    this._item2.ReadFromBytes(data.Slice(8, 4), mode);
    this._item3.ReadFromBytes(data.Slice(12, 4), mode);
  }

  public int Length => 4;
  public int Count => 4;

  public ref SbBitConverter.Tests.Models.TestIntEnum this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<SbBitConverter.Tests.Models.TestIntEnum> AsSpan()
  {
    return CreateSpan(ref _item0, 4);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public Span<SbBitConverter.Tests.Models.TestIntEnum> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateSpan(ref this[start], length);
  }

}
}
#pragma warning restore
