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
[StructLayout(LayoutKind.Explicit, Pack = 16, Size = 48)]
partial struct MyStructArray3
{
  public MyStructArray3(ReadOnlySpan<byte> data, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(data, Unsafe.SizeOf<MyStructArray3>());
    this._item0 = new SbBitConverter.Tests.Models.MyStruct(data.Slice(0, 16), mode);
    this._item1 = new SbBitConverter.Tests.Models.MyStruct(data.Slice(16, 16), mode);
    this._item2 = new SbBitConverter.Tests.Models.MyStruct(data.Slice(32, 16), mode);
  }

  [FieldOffset(0)]private readonly SbBitConverter.Tests.Models.MyStruct _item0;

  [FieldOffset(16)]private readonly SbBitConverter.Tests.Models.MyStruct _item1;

  [FieldOffset(32)]private readonly SbBitConverter.Tests.Models.MyStruct _item2;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public byte[] ToByteArray(BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    var data = new byte[Unsafe.SizeOf<MyStructArray3>()];
    WriteTo(data, mode);
    return data;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public void WriteTo(Span<byte> span, BigAndSmallEndianEncodingMode mode = (BigAndSmallEndianEncodingMode)1)
  {
    CheckLength(span, Unsafe.SizeOf<MyStructArray3>());
    this._item0.WriteTo<SbBitConverter.Tests.Models.MyStruct>(span.Slice(0, 16), mode);
    this._item1.WriteTo<SbBitConverter.Tests.Models.MyStruct>(span.Slice(16, 16), mode);
    this._item2.WriteTo<SbBitConverter.Tests.Models.MyStruct>(span.Slice(32, 16), mode);
  }

  public int Length => 3;
  public int Count => 3;

  public ref readonly SbBitConverter.Tests.Models.MyStruct this[int index]
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    get
    {
      return ref AsSpan()[index];
    }
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ReadOnlySpan<SbBitConverter.Tests.Models.MyStruct> AsSpan()
  {
    return CreateReadOnlySpan(in _item0, 3);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public ReadOnlySpan<SbBitConverter.Tests.Models.MyStruct> Slice(int start, int length)
  {
    if((uint)start > Length || (uint)length > Length - start) throw new ArgumentOutOfRangeException(nameof(start));
    return CreateReadOnlySpan(in this[start], length);
  }

}
}
#pragma warning restore
