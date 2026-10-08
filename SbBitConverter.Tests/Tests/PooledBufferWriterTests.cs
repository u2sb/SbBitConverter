// 补充覆盖：池化 BufferWriter（W4）
using System.Buffers;
using Sb.Extensions.System.Buffers;

namespace SbBitConverter.Tests.Tests;

public class PooledBufferWriterTests
{
  [Fact]
  public void Constructor_RentsBufferAtLeastMinimumLength()
  {
    using var writer = new PooledBufferWriter(100);
    Assert.True(writer.RawBuffer.Length >= 100);
    Assert.Equal(0, writer.WrittenCount);
  }

  [Fact]
  public void GetSpan_Advance_WrittenSpan_BasicWrite()
  {
    using var writer = new PooledBufferWriter(16);
    var span = writer.GetSpan(4);
    Assert.True(span.Length >= 4);

    span[0] = 0x11;
    span[1] = 0x22;
    writer.Advance(2);

    Assert.Equal(2, writer.WrittenCount);
    Assert.Equal(new byte[] { 0x11, 0x22 }, writer.WrittenSpan.ToArray());
    Assert.Equal(new byte[] { 0x11, 0x22 }, writer.WrittenMemory.ToArray());
  }

  [Fact]
  public void GetMemory_Advance_BasicWrite()
  {
    using var writer = new PooledBufferWriter(16);
    var memory = writer.GetMemory(4);
    Assert.True(memory.Length >= 4);

    memory.Span[0] = 0xAA;
    writer.Advance(1);

    Assert.Equal(new byte[] { 0xAA }, writer.WrittenSpan.ToArray());
  }

  [Fact]
  public void Writes_ExpandBufferAndPreserveData()
  {
    using var writer = new PooledBufferWriter(8);
    var originalBuffer = writer.RawBuffer;

    var payload = new byte[100];
    for (var i = 0; i < payload.Length; i++)
    {
      payload[i] = (byte)i;
    }

    // 分多段写入，触发扩容
    for (var offset = 0; offset < payload.Length; offset += 10)
    {
      var count = Math.Min(10, payload.Length - offset);
      payload.AsSpan(offset, count).CopyTo(writer.GetSpan(count));
      writer.Advance(count);
    }

    Assert.Equal(payload, writer.WrittenSpan.ToArray());
    Assert.True(writer.RawBuffer.Length >= payload.Length);
    // 扩容后底层数组应已更换（或至少容量足够）
    if (writer.RawBuffer.Length != originalBuffer.Length)
    {
      Assert.NotSame(originalBuffer, writer.RawBuffer);
    }
  }

  [Fact]
  public void IBufferWriter_Interface_Works()
  {
    using var writer = new PooledBufferWriter(4);
    IBufferWriter<byte> interfaceView = writer;

    var span = interfaceView.GetSpan(2);
    span[0] = 7;
    interfaceView.Advance(1);

    Assert.Equal(1, writer.WrittenCount);
    Assert.Equal(7, writer.WrittenSpan[0]);
  }

  [Fact]
  public void WrittenSpan_GrowsWithAdvance()
  {
    using var writer = new PooledBufferWriter(32);
    var span = writer.GetSpan(8);
    for (var i = 0; i < 3; i++)
    {
      span[i] = 0x5A;
    }
    writer.Advance(3);
    Assert.Equal(3, writer.WrittenSpan.Length);
    Assert.All(writer.WrittenSpan.ToArray(), b => Assert.Equal(0x5A, b));
  }

  [Fact]
  public void Dispose_ThenMembers_ThrowObjectDisposedException()
  {
    var writer = new PooledBufferWriter(16);
    writer.Dispose();

    Assert.Throws<ObjectDisposedException>(() => writer.GetSpan());
    Assert.Throws<ObjectDisposedException>(() => writer.GetMemory());
    Assert.Throws<ObjectDisposedException>(() => writer.Advance(1));
    Assert.Throws<ObjectDisposedException>(() => _ = writer.WrittenSpan);
    Assert.Throws<ObjectDisposedException>(() => _ = writer.WrittenMemory);
    Assert.Throws<ObjectDisposedException>(() => _ = writer.RawBuffer);
  }

  [Fact]
  public void Dispose_IsIdempotent()
  {
    var writer = new PooledBufferWriter(16);
    writer.Dispose();
    writer.Dispose(); // 不应抛异常
  }

  [Fact]
  public void BestArrayPool_RentReturn_ReusesArray()
  {
    var rented = BestArrayPool.Rent(64);
    try
    {
      Assert.True(rented.Length >= 64);
    }
    finally
    {
      BestArrayPool.Return(rented);
    }

    var rentedAgain = BestArrayPool.Rent(64);
    try
    {
      // 私有池应归还同一数组
      Assert.Same(rented, rentedAgain);
    }
    finally
    {
      BestArrayPool.Return(rentedAgain);
    }
  }

  [Fact]
  public void BestArrayPool_Rent_ZeroOrNegative_ReturnsEmpty()
  {
    Assert.Same(Array.Empty<byte>(), BestArrayPool.Rent(0));
    Assert.Same(Array.Empty<byte>(), BestArrayPool.Rent(-1));
  }

  [Fact]
  public void BestArrayPool_Return_NullOrEmpty_NoOp()
  {
    BestArrayPool.Return(null);
    BestArrayPool.Return(Array.Empty<byte>());
  }
}
