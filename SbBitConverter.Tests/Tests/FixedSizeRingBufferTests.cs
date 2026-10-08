// 补充覆盖：对应 Sb.Extensions\System\Buffers\RingBuffers\FixedSizeRingBuffer.cs
using Sb.Extensions.System.Buffers.RingBuffers;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   FixedSizeRingBuffer{T} / RingBufferSpan{T} 测试
/// </summary>
public class FixedSizeRingBufferTests
{
  // ── 构造 ──

  [Theory]
  [InlineData(1, 8)]
  [InlineData(5, 8)]
  [InlineData(8, 8)]
  [InlineData(9, 16)]
  [InlineData(100, 128)]
  public void Constructor_CapacityRoundedUpToPowerOfTwo(int requested, int expected)
  {
    var buffer = new FixedSizeRingBuffer<int>(requested);
    Assert.Equal(expected, buffer.Capacity);
    Assert.Equal(0, buffer.Count);
    Assert.True(buffer.IsEmpty);
  }

  [Fact]
  public void Constructor_ZeroOrNegative_Throws()
  {
    Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeRingBuffer<int>(0));
    Assert.Throws<ArgumentOutOfRangeException>(() => new FixedSizeRingBuffer<int>(-1));
  }

  // ── AddLast ──

  [Fact]
  public void AddLast_NotFull_CountGrows()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLast(1);
    buffer.AddLast(2);
    Assert.Equal(2, buffer.Count);
    Assert.Equal(new[] { 1, 2 }, buffer.ToArray());
  }

  [Fact]
  public void AddLast_WhenFull_OverwritesOldest()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    for (var i = 1; i <= 10; i++) buffer.AddLast(i);

    Assert.Equal(8, buffer.Count);
    Assert.Equal(new[] { 3, 4, 5, 6, 7, 8, 9, 10 }, buffer.ToArray());
  }

  // ── AddLastRange ──

  [Fact]
  public void AddLastRange_EmptySpan_NoOp()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLast(1);
    buffer.AddLastRange(ReadOnlySpan<int>.Empty);
    Assert.Equal(new[] { 1 }, buffer.ToArray());
  }

  [Fact]
  public void AddLastRange_Fits()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    Assert.Equal(new[] { 1, 2, 3 }, buffer.ToArray());
  }

  [Fact]
  public void AddLastRange_LongerThanCapacity_KeepsLast()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLastRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
    Assert.Equal(8, buffer.Count);
    Assert.Equal(new[] { 3, 4, 5, 6, 7, 8, 9, 10 }, buffer.ToArray());
  }

  [Fact]
  public void AddLastRange_WrapsAround_SplitsWrite()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLastRange(new[] { 1, 2, 3, 4, 5, 6 });
    buffer.RemoveFirst(4); // head 前移，剩余 [5,6]
    buffer.AddLastRange(new[] { 7, 8, 9, 10, 11, 12 }); // 空间恰好够，物理写入跨越末尾
    Assert.Equal(8, buffer.Count);
    Assert.Equal(new[] { 5, 6, 7, 8, 9, 10, 11, 12 }, buffer.ToArray());
  }

  // ── AddFirst ──

  [Fact]
  public void AddFirst_OnEmpty()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddFirst(7);
    Assert.Equal(1, buffer.Count);
    Assert.Equal(new[] { 7 }, buffer.ToArray());
  }

  [Fact]
  public void AddFirst_Prepends_AndWrapsHead()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2 });
    buffer.AddFirst(0);
    buffer.AddFirst(-1);
    Assert.Equal(new[] { -1, 0, 1, 2 }, buffer.ToArray());
  }

  [Fact]
  public void AddFirst_WhenFull_DropsOldest()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLastRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
    buffer.AddFirst(0);
    Assert.Equal(8, buffer.Count);
    Assert.Equal(new[] { 0, 1, 2, 3, 4, 5, 6, 7 }, buffer.ToArray());
  }

  // ── RemoveFirst / RemoveLast ──

  [Fact]
  public void RemoveFirst_ReturnsOldestInFifoOrder()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    Assert.Equal(1, buffer.RemoveFirst());
    Assert.Equal(2, buffer.RemoveFirst());
    Assert.Equal(new[] { 3 }, buffer.ToArray());
  }

  [Fact]
  public void RemoveFirst_OnEmpty_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    var ex = Assert.Throws<InvalidOperationException>(() => buffer.RemoveFirst());
    Assert.Contains("empty", ex.Message, StringComparison.OrdinalIgnoreCase);
  }

  [Fact]
  public void RemoveLast_ReturnsNewest()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    Assert.Equal(3, buffer.RemoveLast());
    Assert.Equal(2, buffer.Count);
    Assert.Equal(new[] { 1, 2 }, buffer.ToArray());
  }

  [Fact]
  public void RemoveLast_OnEmpty_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    Assert.Throws<InvalidOperationException>(() => buffer.RemoveLast());
  }

  [Fact]
  public void RemoveFirst_AfterWraparound_HeadAdvancesAcrossBoundary()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    for (var i = 1; i <= 10; i++) buffer.AddLast(i); // 覆盖后 [3..10]，head 已绕圈
    Assert.Equal(3, buffer.RemoveFirst());
    Assert.Equal(4, buffer.RemoveFirst());
    Assert.Equal(new[] { 5, 6, 7, 8, 9, 10 }, buffer.ToArray());
  }

  [Fact]
  public void RemoveFirstN_And_RemoveLastN()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLastRange(new[] { 1, 2, 3, 4, 5 });
    buffer.RemoveFirst(2);
    Assert.Equal(new[] { 3, 4, 5 }, buffer.ToArray());
    buffer.RemoveLast(1);
    Assert.Equal(new[] { 3, 4 }, buffer.ToArray());
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(3)]
  public void RemoveFirstN_Invalid_Throws(int n)
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLast(1);
    buffer.AddLast(2);
    Assert.Throws<ArgumentOutOfRangeException>(() => buffer.RemoveFirst(n));
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(3)]
  public void RemoveLastN_Invalid_Throws(int n)
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    buffer.AddLast(1);
    buffer.AddLast(2);
    Assert.Throws<ArgumentOutOfRangeException>(() => buffer.RemoveLast(n));
  }

  // ── 索引 / 查找 ──

  [Fact]
  public void Indexer_MapsLogicalToPhysical()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    buffer.RemoveFirst(); // head 移动
    Assert.Equal(2, buffer[0]);
    Assert.Equal(3, buffer[1]);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(1)]
  public void Indexer_OutOfRange_Throws(int index)
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLast(1);
    Assert.Throws<ArgumentOutOfRangeException>(() => _ = buffer[index]);
  }

  [Fact]
  public void IndexOf_And_Contains()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 10, 20, 30 });
    Assert.Equal(0, buffer.IndexOf(10));
    Assert.Equal(2, buffer.IndexOf(30));
    Assert.Equal(-1, buffer.IndexOf(99));
    Assert.True(buffer.Contains(20));
    Assert.False(buffer.Contains(99));
  }

  [Fact]
  public void Clear_ResetsCountAndHead()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    for (var i = 1; i <= 6; i++) buffer.AddLast(i);
    buffer.Clear();
    Assert.Equal(0, buffer.Count);
    Assert.True(buffer.IsEmpty);
    Assert.Equal([], buffer.ToArray());
  }

  // ── 枚举 ──

  [Fact]
  public void Enumeration_YieldsInLogicalOrder()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    for (var i = 1; i <= 10; i++) buffer.AddLast(i);
    var listed = new List<int>();
    foreach (var item in (IEnumerable<int>)buffer) listed.Add(item);
    Assert.Equal(new[] { 3, 4, 5, 6, 7, 8, 9, 10 }, listed);
  }

  [Fact]
  public void IReadOnlyList_Indexer_Works()
  {
    IReadOnlyList<int> buffer = new FixedSizeRingBuffer<int>(4);
    Assert.Empty(buffer);
  }

  // ── WrittenSpan / ToArray ──

  [Fact]
  public void ToArray_Empty_ReturnsEmpty()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    Assert.Equal([], buffer.ToArray());
  }

  [Fact]
  public void WrittenSpan_ContiguousData()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    var span = buffer.WrittenSpan;
    Assert.Equal(3, span.Length);
    Assert.Equal(1, span[0]);
    Assert.Equal(3, span[2]);
    Assert.False(span.IsEmpty);
  }

  [Fact]
  public void WrittenSpan_Empty_IsEmpty()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    var span = buffer.WrittenSpan;
    Assert.Equal(0, span.Length);
    Assert.True(span.IsEmpty);
  }

  [Fact]
  public void WrittenSpan_AfterWraparound_PreservesLogicalOrder()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    for (var i = 1; i <= 10; i++) buffer.AddLast(i); // head 绕圈
    var span = buffer.WrittenSpan;
    Assert.Equal(8, span.Length);
    Assert.Equal(new[] { 3, 4, 5, 6, 7, 8, 9, 10 }, span.ToArray());
  }

  // ── RingBufferSpan ──

  [Fact]
  public void RingBufferSpan_CopyTo_And_ToArray()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    var span = buffer.WrittenSpan;
    var dest = new int[3];
    span.CopyTo(dest);
    Assert.Equal(new[] { 1, 2, 3 }, dest);
    Assert.Equal(new[] { 1, 2, 3 }, span.ToArray());
  }

  [Fact]
  public void RingBufferSpan_CopyTo_TooShort_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    Assert.Throws<ArgumentException>(() => buffer.WrittenSpan.CopyTo(new int[2]));
  }

  [Fact]
  public void RingBufferSpan_CopyFrom_WritesThroughToBuffer()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3 });
    buffer.WrittenSpan.CopyFrom(new[] { 7, 8, 9 });
    Assert.Equal(new[] { 7, 8, 9 }, buffer.ToArray());
  }

  [Fact]
  public void RingBufferSpan_CopyFrom_TooLong_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2 });
    Assert.Throws<ArgumentException>(() => buffer.WrittenSpan.CopyFrom(new[] { 1, 2, 3 }));
  }

  [Fact]
  public void RingBufferSpan_Indexer_OutOfRange_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLast(1);
    Assert.Throws<ArgumentOutOfRangeException>(
      () => _ = buffer.WrittenSpan[1]);
    Assert.Throws<ArgumentOutOfRangeException>(
      () => _ = buffer.WrittenSpan[-1]);
  }

  [Fact]
  public void RingBufferSpan_Slice()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2, 3, 4 });
    var span = buffer.WrittenSpan;

    Assert.Equal(new[] { 3, 4 }, span.Slice(2).ToArray());
    Assert.Equal(new[] { 2, 3 }, span.Slice(1, 2).ToArray());
    Assert.Equal(0, span.Slice(4).Length);
  }

  [Fact]
  public void RingBufferSpan_Slice_Invalid_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    buffer.AddLastRange(new[] { 1, 2 });
    Assert.Throws<ArgumentOutOfRangeException>(
      () => _ = buffer.WrittenSpan.Slice(1, 2).Length);
    Assert.Throws<ArgumentOutOfRangeException>(
      () => _ = buffer.WrittenSpan.Slice(-1).Length);
  }

  [Fact]
  public void RingBufferSpan_Enumeration_CrossesSegments()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    for (var i = 1; i <= 10; i++) buffer.AddLast(i); // 数据分为两段
    var listed = new List<int>();
    foreach (var item in buffer.WrittenSpan) listed.Add(item);
    Assert.Equal(new[] { 3, 4, 5, 6, 7, 8, 9, 10 }, listed);
  }

  [Fact]
  public void RingBufferSpan_Empty_ToArray_ReturnsEmpty()
  {
    var buffer = new FixedSizeRingBuffer<int>(4);
    Assert.Equal([], buffer.WrittenSpan.ToArray());
  }

  // ── Stream 操作 ──

  [Fact]
  public void ReadFromStream_ReadsAllBytes()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    using var stream = new MemoryStream([1, 2, 3, 4, 5]);
    var read = buffer.ReadFromStream(stream);
    Assert.Equal(5, read);
    Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, buffer.ToArray());
  }

  [Fact]
  public void ReadFromStream_PartialRead_RespectsAvailableSpace()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    buffer.AddLastRange(new byte[] { 9, 9 });
    using var stream = new MemoryStream([1, 2, 3, 4, 5, 6, 7, 8, 9]);
    var read = buffer.ReadFromStream(stream);
    Assert.Equal(6, read); // 剩余 6 个空位
    Assert.Equal(new byte[] { 9, 9, 1, 2, 3, 4, 5, 6 }, buffer.ToArray());
  }

  [Fact]
  public void ReadFromStream_WhenFull_ReturnsZero()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    buffer.AddLastRange(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
    using var stream = new MemoryStream([1, 2, 3]);
    Assert.Equal(0, buffer.ReadFromStream(stream));
  }

  [Fact]
  public void ReadFromStream_WrongElementType_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    using var stream = new MemoryStream([1, 2, 3]);
    Assert.Throws<InvalidOperationException>(() => buffer.ReadFromStream(stream));
  }

  [Fact]
  public void WriteToStream_WritesAllBytes()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    buffer.AddLastRange(new byte[] { 1, 2, 3, 4 });
    using var stream = new MemoryStream();
    buffer.WriteToStream(stream);
    Assert.Equal(new byte[] { 1, 2, 3, 4 }, stream.ToArray());
  }

  [Fact]
  public void WriteToStream_Wraparound_SplitsIntoTwoSegments()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    for (byte i = 1; i <= 10; i++) buffer.AddLast(i); // [3..10]，物理上分段
    using var stream = new MemoryStream();
    buffer.WriteToStream(stream);
    Assert.Equal(new byte[] { 3, 4, 5, 6, 7, 8, 9, 10 }, stream.ToArray());
  }

  [Fact]
  public void WriteToStream_Empty_WritesNothing()
  {
    var buffer = new FixedSizeRingBuffer<byte>(8);
    using var stream = new MemoryStream();
    buffer.WriteToStream(stream);
    Assert.Equal([], stream.ToArray());
  }

  [Fact]
  public void WriteToStream_WrongElementType_Throws()
  {
    var buffer = new FixedSizeRingBuffer<int>(8);
    using var stream = new MemoryStream();
    Assert.Throws<InvalidOperationException>(() => buffer.WriteToStream(stream));
  }

  [Fact]
  public void StreamRoundTrip_PreservesData()
  {
    var source = new FixedSizeRingBuffer<byte>(8);
    source.AddLastRange(new byte[] { 10, 20, 30 });
    using var stream = new MemoryStream();
    source.WriteToStream(stream);

    var target = new FixedSizeRingBuffer<byte>(8);
    stream.Position = 0;
    target.ReadFromStream(stream);
    Assert.Equal(new byte[] { 10, 20, 30 }, target.ToArray());
  }
}
