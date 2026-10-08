// 补充覆盖：批量转端 Bulk API（W1）
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Sb.Extensions.System;
using Converter = Sb.Extensions.System.SbBitConverter;

namespace SbBitConverter.Tests.Tests;

public class SbBitConverterBulkTests
{
  private static readonly BigAndSmallEndianEncodingMode[] AllModes =
  [
    BigAndSmallEndianEncodingMode.DCBA,
    BigAndSmallEndianEncodingMode.ABCD,
    BigAndSmallEndianEncodingMode.BADC,
    BigAndSmallEndianEncodingMode.CDAB
  ];

  #region 读/写往返

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadWriteBulk_RoundTrip_Int32(BigAndSmallEndianEncodingMode mode)
  {
    int[] values = [1, -2, 3, -4, int.MaxValue, int.MinValue, 0x12345678];
    var bytes = new byte[values.Length * sizeof(int)];

    Converter.WriteBulk(values.AsSpan(), bytes, sizeof(int), mode);

    var result = new int[values.Length];
    Converter.ReadBulk(bytes, result, sizeof(int), mode);

    Assert.Equal(values, result);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadWriteBulk_RoundTrip_Int64(BigAndSmallEndianEncodingMode mode)
  {
    long[] values = [1L, -2L, long.MaxValue, long.MinValue, 0x0123456789ABCDEF];
    var bytes = new byte[values.Length * sizeof(long)];

    Converter.WriteBulk(values.AsSpan(), bytes, sizeof(long), mode);

    var result = new long[values.Length];
    Converter.ReadBulk(bytes, result, sizeof(long), mode);

    Assert.Equal(values, result);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadWriteBulk_RoundTrip_UInt16(BigAndSmallEndianEncodingMode mode)
  {
    ushort[] values = [0x0000, 0xFFFF, 0x1234, 0xABCD, 1];
    var bytes = new byte[values.Length * sizeof(ushort)];

    Converter.WriteBulk(values.AsSpan(), bytes, sizeof(ushort), mode);

    var result = new ushort[values.Length];
    Converter.ReadBulk(bytes, result, sizeof(ushort), mode);

    Assert.Equal(values, result);
  }

  public static TheoryData<BigAndSmallEndianEncodingMode> AllModesMember() =>
    new() { BigAndSmallEndianEncodingMode.DCBA, BigAndSmallEndianEncodingMode.ABCD, BigAndSmallEndianEncodingMode.BADC, BigAndSmallEndianEncodingMode.CDAB };

  #endregion

  #region 与逐元素 ApplyEndianness 结果一致

  [Theory]
  [InlineData(2)]
  [InlineData(4)]
  [InlineData(8)]
  public void WriteBulk_MatchesPerElementApplyEndianness(int elementSize)
  {
    var random = new Random(42);
    var elementCount = 5;
    var values = new byte[elementCount * elementSize];
    random.NextBytes(values);

    var bulkDest = new byte[values.Length];
    Converter.WriteBulk(values, bulkDest, elementSize, BigAndSmallEndianEncodingMode.ABCD);

    // 逐元素：先拷贝，再对每个元素 ApplyEndianness
    var perElementDest = new byte[values.Length];
    values.CopyTo(perElementDest, 0);
    for (var offset = 0; offset < perElementDest.Length; offset += elementSize)
    {
      perElementDest.AsSpan(offset, elementSize).ApplyEndianness(BigAndSmallEndianEncodingMode.ABCD);
    }

    Assert.Equal(perElementDest, bulkDest);
  }

  [Theory]
  [InlineData(2)]
  [InlineData(4)]
  [InlineData(8)]
  public void ReadBulk_MatchesPerElementApplyEndianness(int elementSize)
  {
    var random = new Random(1234);
    var elementCount = 5;
    var source = new byte[elementCount * elementSize];
    random.NextBytes(source);

    var bulkDest = new byte[source.Length];
    Converter.ReadBulk(source, bulkDest, elementSize, BigAndSmallEndianEncodingMode.CDAB);

    var perElementDest = new byte[source.Length];
    source.CopyTo(perElementDest, 0);
    for (var offset = 0; offset < perElementDest.Length; offset += elementSize)
    {
      perElementDest.AsSpan(offset, elementSize).ApplyEndianness(BigAndSmallEndianEncodingMode.CDAB);
    }

    Assert.Equal(perElementDest, bulkDest);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ApplyEndiannessPerElement_MatchesPerElementApplyEndianness(BigAndSmallEndianEncodingMode mode)
  {
    var random = new Random(7);
    var buffer = new byte[3 * 8];
    random.NextBytes(buffer);

    var expected = new byte[buffer.Length];
    buffer.CopyTo(expected, 0);
    for (var offset = 0; offset < expected.Length; offset += 8)
    {
      expected.AsSpan(offset, 8).ApplyEndianness(mode);
    }

    Converter.ApplyEndiannessPerElement(buffer, 8, mode);

    Assert.Equal(expected, buffer);
  }

  #endregion

  #region 与 ToT/WriteTo 逐元素结果等价

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void WriteBulk_EquivalentToPerElementWriteTo(BigAndSmallEndianEncodingMode mode)
  {
    int[] values = [0x11223344, unchecked((int)0xAABBCCDD), 42];

    var bulkDest = new byte[values.Length * sizeof(int)];
    Converter.WriteBulk(values.AsSpan(), bulkDest, sizeof(int), mode);

    var perElementDest = new byte[values.Length * sizeof(int)];
    for (var i = 0; i < values.Length; i++)
    {
      values[i].WriteTo(perElementDest.AsSpan(i * sizeof(int), sizeof(int)), mode);
    }

    Assert.Equal(perElementDest, bulkDest);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadBulk_EquivalentToPerElementToT(BigAndSmallEndianEncodingMode mode)
  {
    // 以 WriteTo 逐元素构造的负载
    int[] values = [0x11223344, unchecked((int)0xAABBCCDD), 42];
    var source = new byte[values.Length * sizeof(int)];
    for (var i = 0; i < values.Length; i++)
    {
      values[i].WriteTo(source.AsSpan(i * sizeof(int), sizeof(int)), mode);
    }

    var bulkResult = new int[values.Length];
    Converter.ReadBulk(source, bulkResult, sizeof(int), mode);

    for (var i = 0; i < values.Length; i++)
    {
      var perElement = source.AsSpan(i * sizeof(int), sizeof(int)).ToT<int>(mode);
      Assert.Equal(perElement, bulkResult[i]);
    }
  }

  #endregion

  #region 异常与边界

  [Fact]
  public void WriteBulk_SourceTooShort_ThrowsInvalidArrayLength()
  {
    var destination = new byte[8];
    Assert.Throws<InvalidArrayLengthException>(() =>
      Converter.WriteBulk(new int[] { 1, 2, 3 }, destination, sizeof(int),
        BigAndSmallEndianEncodingMode.ABCD));
  }

  [Fact]
  public void ReadBulk_SourceTooShort_ThrowsInvalidArrayLength()
  {
    var result = new int[4];
    Assert.Throws<InvalidArrayLengthException>(() =>
      Converter.ReadBulk(new byte[4], result, sizeof(int), BigAndSmallEndianEncodingMode.ABCD));
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ApplyEndiannessPerElement_OddElementSize_ThrowsEvenForDcba(BigAndSmallEndianEncodingMode mode)
  {
    var buffer = new byte[9];
    Assert.Throws<ArgumentException>(() => Converter.ApplyEndiannessPerElement(buffer, 3, mode));
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadBulk_OddElementSize_Throws(BigAndSmallEndianEncodingMode mode)
  {
    var result = new byte[6];
    Assert.Throws<ArgumentException>(() =>
      Converter.ReadBulk(new byte[6], result, 3, mode));
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void WriteBulk_OddElementSize_Throws(BigAndSmallEndianEncodingMode mode)
  {
    var values = new byte[6];
    Assert.Throws<ArgumentException>(() =>
      Converter.WriteBulk(values, new byte[6], 3, mode));
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ReadBulk_EmptyBuffer_NoOp(BigAndSmallEndianEncodingMode mode)
  {
    var result = Array.Empty<int>();
    Converter.ReadBulk(ReadOnlySpan<byte>.Empty, result, sizeof(int), mode);
    Assert.Empty(result);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void WriteBulk_EmptyBuffer_NoOp(BigAndSmallEndianEncodingMode mode)
  {
    Converter.WriteBulk(ReadOnlySpan<int>.Empty, Span<byte>.Empty, sizeof(int), mode);
  }

  [Theory]
  [MemberData(nameof(AllModesMember))]
  public void ApplyEndiannessPerElement_EmptyBuffer_NoOp(BigAndSmallEndianEncodingMode mode)
  {
    Converter.ApplyEndiannessPerElement(Span<byte>.Empty, 4, mode);
  }

  [Fact]
  public void ApplyEndiannessPerElement_SingleByteElement_NoOp()
  {
    var buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
    var expected = (byte[])buffer.Clone();
    foreach (var mode in AllModes)
    {
      var copy = (byte[])buffer.Clone();
      Converter.ApplyEndiannessPerElement(copy, 1, mode);
      Assert.Equal(expected, copy);
    }
  }

  [Fact]
  public void ReadBulk_ElementSizeOne_CopiesAsIs()
  {
    byte[] source = [1, 2, 3, 4, 5];
    var result = new byte[5];
    foreach (var mode in AllModes)
    {
      Converter.ReadBulk(source, result, 1, mode);
      Assert.Equal(source, result);
    }
  }

  [Fact]
  public void ReadBulk_ExtraSourceBytes_OnlyReadsNeeded()
  {
    int[] expected = [0x04030201, 0x08070605];
    var source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 0xFF, 0xFF };
    var result = new int[2];

    Converter.ReadBulk(source, result, sizeof(int), BigAndSmallEndianEncodingMode.DCBA);

    Assert.Equal(expected, result);
  }

  [Fact]
  public void ApplyEndiannessPerElement_InvalidMode_ThrowsArgumentOutOfRange()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() =>
      Converter.ApplyEndiannessPerElement(new byte[4], 4, (BigAndSmallEndianEncodingMode)99));
  }

  #endregion
}
